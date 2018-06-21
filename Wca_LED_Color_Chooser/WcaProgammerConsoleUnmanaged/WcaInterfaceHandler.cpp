#include "StdAfx.h"
#include <iostream>
#include "WcaInterfaceHandler.h"

using namespace std;


WcaInterfaceHandler::WcaInterfaceHandler(void)
{
	m_pWcaInterface = NULL;
	m_IsInitialized = false;
}

WcaInterfaceHandler::~WcaInterfaceHandler(void)
{

}

bool WcaInterfaceHandler::Initialize(const char* pCOMPort, int baudrate)
{
	if(!m_IsInitialized)
	{
		VARIANT_BOOL res = FALSE;
		::CoInitialize(NULL);
		HRESULT hres = ::CoCreateInstance(CLSID_WcaInterface,NULL,CLSCTX_ALL,IID_IWcaInterface,(void**)&m_pWcaInterface);

		if(!FAILED(hres))
		{
			m_pWcaInterface->Open(WcaInterfaceType_UART,CComBSTR(pCOMPort),baudrate,&res);

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

void WcaInterfaceHandler::DeInitialize()
{
	if(m_IsInitialized)
	{
		::CoUninitialize();
		m_IsInitialized = false;
		m_pWcaInterface->Close();
		
	}
}

WcaInterfaceCommandResult WcaInterfaceHandler::SendCommandSync_2(byte destinationAddress, byte command, byte data[], int length, byte** prdata, int size, int *written)
{
	WcaInterfaceCommandResult res;
	*written = 0;
	
	if(m_IsInitialized)
	{
		SAFEARRAYBOUND aBound[1];
		aBound[0].lLbound = 0;
		aBound[0].cElements = length;
		SAFEARRAY *sf_in = SafeArrayCreate(VT_UI1,1,&aBound[0]);
		sf_in->pvData = data;
		SAFEARRAY *sf_out = NULL;

		m_pWcaInterface->SendCommandSync_2(WcaInterfaceAddress_PROGRAMMER,command,sf_in,&sf_out,&res);
		
		if (res == WcaInterfaceCommandResult_PosAck)
		{
			if (sf_out != NULL)
			{
				byte * bp = (byte*)sf_out->pvData;
				for (int i = sf_out->rgsabound[0].lLbound ; i < sf_out->rgsabound[0].cElements && i<size; ++i)
				{
					*(*prdata+i) = *(bp + i);
					*written = *written +1;
				}
			}
		}

		//SafeArrayDestroy(sf_in);
	}
	return res;
}

WcaInterfaceCommandResult WcaInterfaceHandler::SendCommandSync(byte destinationAddress, byte command, byte** prdata, int size, int *written)
{
	WcaInterfaceCommandResult res;
	*written = 0;
	
	if(m_IsInitialized)
	{
		SAFEARRAY *sf_out = NULL;
		m_pWcaInterface->SendCommandSync(WcaInterfaceAddress_PROGRAMMER,command,&sf_out,&res);
	
		if (res == WcaInterfaceCommandResult_PosAck)
		{
			if (sf_out != NULL)
			{
				byte * bp = (byte*)sf_out->pvData;
				for (int i = sf_out->rgsabound[0].lLbound ; i < sf_out->rgsabound[0].cElements && i<size; ++i)
				{
					*(*prdata+i) = *(bp + i);
					*written = *written +1;
				}
			}
		}
	}
	return res;
}