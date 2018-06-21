#include "StdAfx.h"
#include <iostream>
#include "WCAInterfaceBootloaderHandler.h"

using namespace std;


WCAInterfaceBootloaderHandler::WCAInterfaceBootloaderHandler(void)
{
	m_pWCAInterfaceBootloader = NULL;
	m_IsInitialized = false;
}

WCAInterfaceBootloaderHandler::~WCAInterfaceBootloaderHandler(void)
{

}

bool WCAInterfaceBootloaderHandler::Initialize(char* pCOMPort)
{
	
	if(!m_IsInitialized)
	{
		VARIANT_BOOL res = FALSE;
		::CoInitialize(NULL);
		HRESULT hres = ::CoCreateInstance(CLSID_WCAInterfaceBootloader,NULL,CLSCTX_ALL,IID_IWCAInterfaceBootloader,(void**)&m_pWCAInterfaceBootloader);

		if(!FAILED(hres))
		{
			m_pWCAInterfaceBootloader->Open(CComBSTR(pCOMPort),&res);

			if (res)
			{
				m_IsInitialized = true;
			}
		}

		if(!m_IsInitialized)
		{
			::CoUninitialize();
		}
	}
	return m_IsInitialized;
}

void WCAInterfaceBootloaderHandler::DeInitialize()
{
	if(m_IsInitialized)
	{
		m_IsInitialized = false;
		m_pWCAInterfaceBootloader->Close();
		m_pWCAInterfaceBootloader->Release();
		::CoUninitialize();
	}
}

bool WCAInterfaceBootloaderHandler::Connect()
{
	VARIANT_BOOL res = FALSE;
	if(m_IsInitialized)
	{
		m_pWCAInterfaceBootloader->Login(&res);
	}
	return res?true:false;
}

char* WCAInterfaceBootloaderHandler::ReadBootloaderParameter()
{
	char *p_c = NULL;
	if(m_IsInitialized)
	{
		BSTR str;
		m_pWCAInterfaceBootloader->ReadBootloaderParameter(&str);
		p_c = _com_util::ConvertBSTRToString(str);
	}
	return p_c;
}

char* WCAInterfaceBootloaderHandler::ReadApplicationParameter()
{
	char *p_c = NULL;
	if(m_IsInitialized)
	{
		BSTR str;
		m_pWCAInterfaceBootloader->ReadApplicationParameter(&str);
		p_c = _com_util::ConvertBSTRToString(str);
	}
	return p_c;
}

bool WCAInterfaceBootloaderHandler::StartApplication(unsigned short start_param)
{
	VARIANT_BOOL res = FALSE;
	if(m_IsInitialized)
	{
		m_pWCAInterfaceBootloader->StartApplication(start_param,&res);
	}
	return res?true:false;
}

