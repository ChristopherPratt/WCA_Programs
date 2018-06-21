#pragma once
#include "device.h"
#include "WcaInterfaceHandler.h"

class WCADevice :
	public Device
{

	public:
		WCADevice(void);
		virtual ~WCADevice(void);
};
