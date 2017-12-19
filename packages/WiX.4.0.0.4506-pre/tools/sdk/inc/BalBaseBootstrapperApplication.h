// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.

#include <windows.h>
#include <msiquery.h>

#include "BootstrapperEngine.h"
#include "BootstrapperApplication.h"
#include "IBootstrapperEngine.h"
#include "IBootstrapperApplication.h"

#include "balutil.h"
#include "balretry.h"

class CBalBaseBootstrapperApplication : public IBootstrapperApplication
{
public: // IUnknown
    virtual STDMETHODIMP QueryInterface(
        __in REFIID riid,
        __out LPVOID *ppvObject
        )
    {
        if (!ppvObject)
        {
            return E_INVALIDARG;
        }

        *ppvObject = NULL;

        if (::IsEqualIID(__uuidof(IBootstrapperApplication), riid))
        {
            *ppvObject = static_cast<IBootstrapperApplication*>(this);
        }
        else if (::IsEqualIID(IID_IUnknown, riid))
        {
            *ppvObject = static_cast<IUnknown*>(this);
        }
        else // no interface for requested iid
        {
            return E_NOINTERFACE;
        }

        AddRef();
        return S_OK;
    }

    virtual STDMETHODIMP_(ULONG) AddRef()
    {
        return ::InterlockedIncrement(&this->m_cReferences);
    }

    virtual STDMETHODIMP_(ULONG) Release()
    {
        long l = ::InterlockedDecrement(&this->m_cReferences);
        if (0 < l)
        {
            return l;
        }

        delete this;
        return 0;
    }

public: // IBootstrapperApplication
    virtual STDMETHODIMP OnStartup()
    {
        return S_OK;
    }

    virtual STDMETHODIMP OnShutdown(
        __inout BOOTSTRAPPER_SHUTDOWN_ACTION* /*pAction*/
        )
    {
        return S_OK;
    }

    virtual STDMETHODIMP OnSystemShutdown(
        __in DWORD dwEndSession,
        __inout BOOL* pfCancel
        )
    {
        HRESULT hr = S_OK;

        // Allow requests to shut down when critical or not applying.
        *pfCancel = !(ENDSESSION_CRITICAL & dwEndSession || !m_fApplying);

        return hr;
    }

    virtual STDMETHODIMP OnDetectBegin(
        __in BOOL /*fInstalled*/,
        __in DWORD /*cPackages*/,
        __inout BOOL* pfCancel
        )
    {
        *pfCancel |= CheckCanceled();
        return S_OK;
    }

    virtual STDMETHODIMP OnDetectForwardCompatibleBundle(
        __in_z LPCWSTR /*wzBundleId*/,
        __in BOOTSTRAPPER_RELATION_TYPE /*relationType*/,
        __in_z LPCWSTR /*wzBundleTag*/,
        __in BOOL /*fPerMachine*/,
        __in DWORD64 /*dw64Version*/,
        __inout BOOL* pfCancel,
        __inout BOOL* /*pfIgnoreBundle*/
        )
    {
        *pfCancel |= CheckCanceled();
        return S_OK;
    }

    virtual STDMETHODIMP OnDetectUpdateBegin(
        __in_z LPCWSTR /*wzUpdateLocation*/,
        __inout BOOL* pfCancel,
        __inout BOOL* /*pfSkip*/
        )
    {
        *pfCancel |= CheckCanceled();
        return S_OK;
    }

    virtual STDMETHODIMP OnDetectUpdate(
        __in_z LPCWSTR /*wzUpdateLocation*/,
        __in DWORD64 /*dw64Size*/,
        __in DWORD64 /*dw64Version*/,
        __in_z LPCWSTR /*wzTitle*/,
        __in_z LPCWSTR /*wzSummary*/,
        __in_z LPCWSTR /*wzContentType*/,
        __in_z LPCWSTR /*wzContent*/,
        __inout BOOL* pfCancel,
        __inout BOOL* /*pfStopProcessingUpdates*/
        )
    {
        *pfCancel |= CheckCanceled();
        return S_OK;
    }

    virtual STDMETHODIMP OnDetectUpdateComplete(
        __in HRESULT /*hrStatus*/,
        __inout BOOL* /*pfIgnoreError*/
        )
    {
        return S_OK;
    }

    virtual STDMETHODIMP OnDetectRelatedBundle(
        __in_z LPCWSTR /*wzBundleId*/,
        __in BOOTSTRAPPER_RELATION_TYPE /*relationType*/,
        __in_z LPCWSTR /*wzBundleTag*/,
        __in BOOL /*fPerMachine*/,
        __in DWORD64 /*dw64Version*/,
        __in BOOTSTRAPPER_RELATED_OPERATION /*operation*/,
        __inout BOOL* pfCancel
        )
    {
        *pfCancel |= CheckCanceled();
        return S_OK;
    }

    virtual STDMETHODIMP OnDetectPackageBegin(
        __in_z LPCWSTR /*wzPackageId*/,
        __inout BOOL* pfCancel
        )
    {
        *pfCancel |= CheckCanceled();
        return S_OK;
    }

    virtual STDMETHODIMP OnDetectCompatiblePackage(
        __in_z LPCWSTR /*wzPackageId*/,
        __in_z LPCWSTR /*wzCompatiblePackageId*/,
        __inout BOOL* pfCancel
        )
    {
        *pfCancel |= CheckCanceled();
        return S_OK;
    }

    virtual STDMETHODIMP OnDetectRelatedMsiPackage(
        __in_z LPCWSTR /*wzPackageId*/,
        __in_z LPCWSTR /*wzUpgradeCode*/,
        __in_z LPCWSTR /*wzProductCode*/,
        __in BOOL /*fPerMachine*/,
        __in DWORD64 /*dw64Version*/,
        __in BOOTSTRAPPER_RELATED_OPERATION /*operation*/,
        __inout BOOL* pfCancel
        ) 
    {
        *pfCancel |= CheckCanceled();
        return S_OK;
    }

    virtual STDMETHODIMP OnDetectTargetMsiPackage(
        __in_z LPCWSTR /*wzPackageId*/,
        __in_z LPCWSTR /*wzProductCode*/,
        __in BOOTSTRAPPER_PACKAGE_STATE /*patchState*/,
        __inout BOOL* pfCancel
        )
    {
        *pfCancel |= CheckCanceled();
        return S_OK;
    }

    virtual STDMETHODIMP OnDetectMsiFeature(
        __in_z LPCWSTR /*wzPackageId*/,
        __in_z LPCWSTR /*wzFeatureId*/,
        __in BOOTSTRAPPER_FEATURE_STATE /*state*/,
        __inout BOOL* pfCancel
        )
    {
        *pfCancel |= CheckCanceled();
        return S_OK;
    }

    virtual STDMETHODIMP OnDetectPackageComplete(
        __in_z LPCWSTR /*wzPackageId*/,
        __in HRESULT /*hrStatus*/,
        __in BOOTSTRAPPER_PACKAGE_STATE /*state*/
        )
    {
        return S_OK;
    }

    virtual STDMETHODIMP OnDetectComplete(
        __in HRESULT /*hrStatus*/
        )
    {
        return S_OK;
    }

    virtual STDMETHODIMP OnPlanBegin(
        __in DWORD /*cPackages*/,
        __inout BOOL* pfCancel
        )
    {
        *pfCancel |= CheckCanceled();
        return S_OK;
    }

    virtual STDMETHODIMP_(int) OnPlanRelatedBundle(
        __in_z LPCWSTR /*wzBundleId*/,
        __inout BOOTSTRAPPER_REQUEST_STATE* /*pRequestedState*/
        )
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnPlanPackageBegin(
        __in_z LPCWSTR /*wzPackageId*/, 
        __inout BOOTSTRAPPER_REQUEST_STATE* /*pRequestState*/
        )
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnPlanCompatiblePackage(
        __in_z LPCWSTR /*wzPackageId*/,
        __inout BOOTSTRAPPER_REQUEST_STATE* /*pRequestedState*/
        )
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnPlanTargetMsiPackage(
        __in_z LPCWSTR /*wzPackageId*/,
        __in_z LPCWSTR /*wzProductCode*/,
        __inout BOOTSTRAPPER_REQUEST_STATE* /*pRequestedState*/
        )
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnPlanMsiFeature(
        __in_z LPCWSTR /*wzPackageId*/,
        __in_z LPCWSTR /*wzFeatureId*/,
        __inout BOOTSTRAPPER_FEATURE_STATE* /*pRequestedState*/
        )
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(void) OnPlanPackageComplete(
        __in_z LPCWSTR /*wzPackageId*/,
        __in HRESULT /*hrStatus*/,
        __in BOOTSTRAPPER_PACKAGE_STATE /*state*/,
        __in BOOTSTRAPPER_REQUEST_STATE /*requested*/,
        __in BOOTSTRAPPER_ACTION_STATE /*execute*/,
        __in BOOTSTRAPPER_ACTION_STATE /*rollback*/
        )
    {
    }

    virtual STDMETHODIMP OnPlanComplete(
        __in HRESULT /*hrStatus*/
        )
    {
        return S_OK;
    }

    virtual STDMETHODIMP_(int) OnApplyBegin(
        __in DWORD /*dwPhaseCount*/
        )
    {
        m_fApplying = TRUE;

        m_dwProgressPercentage = 0;
        m_dwOverallProgressPercentage = 0;

        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnElevate()
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnRegisterBegin()
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(void) OnRegisterComplete(
        __in HRESULT /*hrStatus*/
        )
    {
        return;
    }

    virtual STDMETHODIMP_(void) OnUnregisterBegin()
    {
        return;
    }

    virtual STDMETHODIMP_(void) OnUnregisterComplete(
        __in HRESULT /*hrStatus*/
        )
    {
        return;
    }

    virtual STDMETHODIMP_(int) OnApplyComplete(
        __in HRESULT /*hrStatus*/,
        __in BOOTSTRAPPER_APPLY_RESTART restart
        )
    {
        m_fApplying = FALSE;
        return BOOTSTRAPPER_APPLY_RESTART_REQUIRED == restart ? IDRESTART : CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnCacheBegin()
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnCachePackageBegin(
        __in_z LPCWSTR /*wzPackageId*/,
        __in DWORD /*cCachePayloads*/,
        __in DWORD64 /*dw64PackageCacheSize*/
        )
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnCacheAcquireBegin(
        __in_z LPCWSTR wzPackageOrContainerId,
        __in_z_opt LPCWSTR wzPayloadId,
        __in BOOTSTRAPPER_CACHE_OPERATION /*operation*/,
        __in_z LPCWSTR /*wzSource*/
        )
    {
        BalRetryStartPackage(BALRETRY_TYPE_CACHE, wzPackageOrContainerId, wzPayloadId);
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnCacheAcquireProgress(
        __in_z LPCWSTR /*wzPackageOrContainerId*/,
        __in_z_opt LPCWSTR /*wzPayloadId*/,
        __in DWORD64 /*dw64Progress*/,
        __in DWORD64 /*dw64Total*/,
        __in DWORD /*dwOverallPercentage*/
        )
    {
        HRESULT hr = S_OK;
        int nResult = IDNOACTION;

        // Send progress even though we don't update the numbers to at least give the caller an opportunity
        // to cancel.
        if (BOOTSTRAPPER_DISPLAY_EMBEDDED == m_display)
        {
            hr = m_pEngine->SendEmbeddedProgress(m_dwProgressPercentage, m_dwOverallProgressPercentage, &nResult);
            BalExitOnFailure(hr, "Failed to send embedded cache progress.");
        }

    LExit:
        return FAILED(hr) ? IDERROR : CheckCanceled() ? IDCANCEL : nResult;
    }

    virtual STDMETHODIMP_(int) OnCacheAcquireComplete(
        __in_z LPCWSTR wzPackageOrContainerId,
        __in_z_opt LPCWSTR wzPayloadId,
        __in HRESULT hrStatus,
        __in int nRecommendation
        )
    {
        int nResult = CheckCanceled() ? IDCANCEL : BalRetryEndPackage(BALRETRY_TYPE_CACHE, wzPackageOrContainerId, wzPayloadId, hrStatus);
        return IDNOACTION == nResult ? nRecommendation : nResult;
    }

    virtual STDMETHODIMP_(int) OnCacheVerifyBegin(
        __in_z LPCWSTR /*wzPackageId*/,
        __in_z LPCWSTR /*wzPayloadId*/
        )
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnCacheVerifyComplete(
        __in_z LPCWSTR /*wzPackageId*/,
        __in_z LPCWSTR /*wzPayloadId*/,
        __in HRESULT /*hrStatus*/,
        __in int nRecommendation
        )
    {
        return CheckCanceled() ? IDCANCEL : nRecommendation;
    }

    virtual STDMETHODIMP_(int) OnCachePackageComplete(
        __in_z LPCWSTR /*wzPackageId*/,
        __in HRESULT /*hrStatus*/,
        __in int nRecommendation
        )
    {
        return CheckCanceled() ? IDCANCEL : nRecommendation;
    }

    virtual STDMETHODIMP_(void) OnCacheComplete(
        __in HRESULT /*hrStatus*/
        )
    {
    }

    virtual STDMETHODIMP_(int) OnExecuteBegin(
        __in DWORD /*cExecutingPackages*/
        )
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnExecutePackageBegin(
        __in_z LPCWSTR wzPackageId,
        __in BOOL fExecute
        )
    {
        // Only track retry on execution (not rollback).
        if (fExecute)
        {
            BalRetryStartPackage(BALRETRY_TYPE_EXECUTE, wzPackageId, NULL);
        }

        m_fRollingBack = !fExecute;
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnExecutePatchTarget(
        __in_z LPCWSTR /*wzPackageId*/,
        __in_z LPCWSTR /*wzTargetProductCode*/
        )
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnError(
        __in BOOTSTRAPPER_ERROR_TYPE errorType,
        __in_z LPCWSTR wzPackageId,
        __in DWORD dwCode,
        __in_z LPCWSTR /*wzError*/,
        __in DWORD /*dwUIHint*/,
        __in DWORD /*cData*/,
        __in_ecount_z_opt(cData) LPCWSTR* /*rgwzData*/,
        __in int nRecommendation
        )
    {
        BalRetryErrorOccurred(wzPackageId, dwCode);

        if (BOOTSTRAPPER_DISPLAY_FULL == m_display)
        {
            if (BOOTSTRAPPER_ERROR_TYPE_HTTP_AUTH_SERVER == errorType ||BOOTSTRAPPER_ERROR_TYPE_HTTP_AUTH_PROXY == errorType)
            {
                nRecommendation = IDTRYAGAIN;
            }
        }

        return CheckCanceled() ? IDCANCEL : nRecommendation;
    }

    virtual STDMETHODIMP_(int) OnProgress(
        __in DWORD dwProgressPercentage,
        __in DWORD dwOverallProgressPercentage
        )
    {
        HRESULT hr = S_OK;
        int nResult = IDNOACTION;

        m_dwProgressPercentage = dwProgressPercentage;
        m_dwOverallProgressPercentage = dwOverallProgressPercentage;

        if (BOOTSTRAPPER_DISPLAY_EMBEDDED == m_display)
        {
            hr = m_pEngine->SendEmbeddedProgress(m_dwProgressPercentage, m_dwOverallProgressPercentage, &nResult);
            BalExitOnFailure(hr, "Failed to send embedded overall progress.");
        }

    LExit:
        return FAILED(hr) ? IDERROR : CheckCanceled() ? IDCANCEL : nResult;
    }

    virtual STDMETHODIMP_(int) OnExecuteProgress(
        __in_z LPCWSTR /*wzPackageId*/,
        __in DWORD /*dwProgressPercentage*/,
        __in DWORD /*dwOverallProgressPercentage*/
        )
    {
        HRESULT hr = S_OK;
        int nResult = IDNOACTION;

        // Send progress even though we don't update the numbers to at least give the caller an opportunity
        // to cancel.
        if (BOOTSTRAPPER_DISPLAY_EMBEDDED == m_display)
        {
            hr = m_pEngine->SendEmbeddedProgress(m_dwProgressPercentage, m_dwOverallProgressPercentage, &nResult);
            BalExitOnFailure(hr, "Failed to send embedded execute progress.");
        }

    LExit:
        return FAILED(hr) ? IDERROR : CheckCanceled() ? IDCANCEL : nResult;
    }

    virtual STDMETHODIMP_(int) OnExecuteMsiMessage(
        __in_z LPCWSTR /*wzPackageId*/,
        __in INSTALLMESSAGE /*mt*/,
        __in UINT /*uiFlags*/,
        __in_z LPCWSTR /*wzMessage*/,
        __in DWORD /*cData*/,
        __in_ecount_z_opt(cData) LPCWSTR* /*rgwzData*/,
        __in int nRecommendation
        )
    {
        return CheckCanceled() ? IDCANCEL : nRecommendation;
    }

    virtual STDMETHODIMP_(int) OnExecuteFilesInUse(
        __in_z LPCWSTR /*wzPackageId*/,
        __in DWORD /*cFiles*/,
        __in_ecount_z(cFiles) LPCWSTR* /*rgwzFiles*/
        )
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnExecutePackageComplete(
        __in_z LPCWSTR wzPackageId,
        __in HRESULT hrExitCode,
        __in BOOTSTRAPPER_APPLY_RESTART /*restart*/,
        __in int nRecommendation
        )
    {
        int nResult = CheckCanceled() ? IDCANCEL : BalRetryEndPackage(BALRETRY_TYPE_EXECUTE, wzPackageId, NULL, hrExitCode);
        return IDNOACTION == nResult ? nRecommendation : nResult;
    }

    virtual STDMETHODIMP_(void) OnExecuteComplete(
        __in HRESULT /*hrStatus*/
        )
    {
    }

    virtual STDMETHODIMP_(int) OnResolveSource(
        __in_z LPCWSTR /*wzPackageOrContainerId*/,
        __in_z_opt LPCWSTR /*wzPayloadId*/,
        __in_z LPCWSTR /*wzLocalSource*/,
        __in_z_opt LPCWSTR /*wzDownloadSource*/
        )
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(int) OnLaunchApprovedExeBegin()
    {
        return CheckCanceled() ? IDCANCEL : IDNOACTION;
    }

    virtual STDMETHODIMP_(void) OnLaunchApprovedExeComplete(
        __in HRESULT /*hrStatus*/,
        __in DWORD /*dwProcessId*/
        )
    {
    }

    virtual STDMETHODIMP_(HRESULT) BAProc(
        __in BOOTSTRAPPER_APPLICATION_MESSAGE /*message*/,
        __in const LPVOID /*pvArgs*/,
        __inout LPVOID /*pvResults*/,
        __in_opt LPVOID /*pvContext*/
        )
    {
        return E_NOTIMPL;
    }

    virtual STDMETHODIMP_(void) BAProcFallback(
        __in BOOTSTRAPPER_APPLICATION_MESSAGE /*message*/,
        __in const LPVOID /*pvArgs*/,
        __inout LPVOID /*pvResults*/,
        __inout HRESULT* /*phr*/,
        __in_opt LPVOID /*pvContext*/
        )
    {
    }

protected:
    //
    // PromptCancel - prompts the user to close (if not forced).
    //
    virtual BOOL PromptCancel(
        __in HWND hWnd,
        __in BOOL fForceCancel,
        __in_z_opt LPCWSTR wzMessage,
        __in_z_opt LPCWSTR wzCaption
        )
    {
        ::EnterCriticalSection(&m_csCanceled);

        // Only prompt the user to close if we have not canceled already.
        if (!m_fCanceled)
        {
            if (fForceCancel)
            {
                m_fCanceled = TRUE;
            }
            else
            {
                m_fCanceled = (IDYES == ::MessageBoxW(hWnd, wzMessage, wzCaption, MB_YESNO | MB_ICONEXCLAMATION));
            }
        }

        ::LeaveCriticalSection(&m_csCanceled);

        return m_fCanceled;
    }

    //
    // CheckCanceled - waits if the cancel dialog is up and checks to see if the user canceled the operation.
    //
    BOOL CheckCanceled()
    {
        ::EnterCriticalSection(&m_csCanceled);
        ::LeaveCriticalSection(&m_csCanceled);
        return m_fRollingBack ? FALSE : m_fCanceled;
    }

    BOOL IsRollingBack()
    {
        return m_fRollingBack;
    }

    BOOL IsCanceled()
    {
        return m_fCanceled;
    }

    CBalBaseBootstrapperApplication(
        __in IBootstrapperEngine* pEngine,
        __in const BOOTSTRAPPER_CREATE_ARGS* pArgs,
        __in DWORD dwRetryCount = 0,
        __in DWORD dwRetryTimeout = 1000
        )
    {
        m_cReferences = 1;
        m_display = pArgs->pCommand->display;
        m_restart = pArgs->pCommand->restart;

        pEngine->AddRef();
        m_pEngine = pEngine;

        ::InitializeCriticalSection(&m_csCanceled);
        m_fCanceled = FALSE;
        m_fApplying = FALSE;
        m_fRollingBack = FALSE;

        BalRetryInitialize(dwRetryCount, dwRetryTimeout);
    }

    virtual ~CBalBaseBootstrapperApplication()
    {
        BalRetryUninitialize();
        ::DeleteCriticalSection(&m_csCanceled);

        ReleaseNullObject(m_pEngine);
    }

protected:
    CRITICAL_SECTION m_csCanceled;
    BOOL m_fCanceled;

private:
    long m_cReferences;
    BOOTSTRAPPER_DISPLAY m_display;
    BOOTSTRAPPER_RESTART m_restart;
    IBootstrapperEngine* m_pEngine;

    BOOL m_fApplying;
    BOOL m_fRollingBack;

    DWORD m_dwProgressPercentage;
    DWORD m_dwOverallProgressPercentage;
};