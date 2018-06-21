#include "StdAfx.h"
#include "ProgrammingDevice.h"

const SIGNAL_ASSIGNMENT signal_array[HS_MAX_SIGNAL] =
{
	{GPIO_A,BIT7,PD_INPUT}, //HS_TEL_ON_TRIGGER_BUTTON,
	{GPIO_A,BIT6,PD_INPUT}, //HS_TEL_ON_TRIGGER_EXTERN,
	{GPIO_A,BIT5,PD_INPUT}, //HS_KESSY_TRIGGER_BUTTON,
	{GPIO_B,BIT6,PD_OUTPUT}, //HS_KESSY_LED1,
	{GPIO_B,BIT5,PD_OUTPUT}, //HS_KESSY_LED2,
	{GPIO_B,BIT4,PD_OUTPUT}, //HS_KESSY_LED3,
	{GPIO_B,BIT0,PD_OUTPUT}, //HS_KESSY_LED4,
	{GPIO_A,BIT0,PD_OUTPUT}, //HS_GENERAL_LED1,
	{GPIO_A,BIT1,PD_OUTPUT}, //HS_GENERAL_LED2,
	{GPIO_A,BIT2,PD_OUTPUT}, //HS_GENERAL_LED3,
	{GPIO_A,BIT4,PD_OUTPUT}, //HS_GENERAL_LED4,
	{GPIO_C,BIT15,PD_OUTPUT}, //HS_UNIVERSAL_OUTPUT,
	{GPIO_C,BIT14,PD_OUTPUT}, //HS_EXTERN_TEL_ON_ENABLE,
	{GPIO_E,BIT7,PD_INPUT}, //HS_ENCODER_A,
	{GPIO_E,BIT6,PD_INPUT}, //HS_ENCODER_B,
	{GPIO_E,BIT5,PD_OUTPUT}, //HS_KESSY_SIGNAL_OUTPUT,
	{GPIO_E,BIT4,PD_INPUT}, //HS_ENCODER_PUSH_BUTTON,
	{GPIO_C,BIT13,PD_OUTPUT}, //HS_TEL_ON_SIGNAL,//TelOn_Mc
	{GPIO_E,BIT3,PD_OUTPUT}, //HS_SELECT_RXD_TXD,
	{GPIO_E,BIT2,PD_OUTPUT}, //HS_SELECT_TEL_ON_RXD,
	{GPIO_E,BIT1,PD_OUTPUT}, //HS_12V_SWITCH,
	{GPIO_E,BIT0,PD_OUTPUT}, //HS_ENABLE_PWM,
	{GPIO_C,BIT3,PD_INPUT} //HS_PWM_DATA_MONITOR,
};


ProgrammingDevice::ProgrammingDevice(void) : Device(0x01)
{
}

ProgrammingDevice::~ProgrammingDevice(void)
{
}


bool ProgrammingDevice::SimulatePressStartButton(int press_time_ms)
{
	bool result = true;

	SetOutput(signal_array[HS_TEL_ON_SIGNAL].port,signal_array[HS_TEL_ON_SIGNAL].pin,LOW);			//+12V on
    SetOutput(signal_array[HS_SELECT_TEL_ON_RXD].port,signal_array[HS_SELECT_TEL_ON_RXD].pin,HIGH);	//+12V on TELON WCA
	SetOutput(signal_array[HS_GENERAL_LED1].port,signal_array[HS_GENERAL_LED2].pin,HIGH);				//LED1 on

	Sleep(press_time_ms);

	SetOutput(signal_array[HS_TEL_ON_SIGNAL].port,signal_array[HS_TEL_ON_SIGNAL].pin,HIGH);			//+12V off
	SetOutput(signal_array[HS_SELECT_TEL_ON_RXD].port,signal_array[HS_SELECT_TEL_ON_RXD].pin,LOW); 	//TXD_A on TELON WCA
	SetOutput(signal_array[HS_GENERAL_LED1].port,signal_array[HS_GENERAL_LED2].pin,LOW);	
	
	Sleep(4000);

	SetOutput(signal_array[HS_SELECT_TEL_ON_RXD].port,signal_array[HS_SELECT_TEL_ON_RXD].pin,HIGH);//LED1 off

	return result;
}

bool ProgrammingDevice::SimulatePressProgStartButton(int press_time_ms)
{
	bool result = true;

	SetOutput(signal_array[HS_TEL_ON_SIGNAL].port,signal_array[HS_TEL_ON_SIGNAL].pin,HIGH);			//+12V on
	SetOutput(signal_array[HS_SELECT_TEL_ON_RXD].port,signal_array[HS_SELECT_TEL_ON_RXD].pin,LOW);	//+12V on TELON WCA
	SetOutput(signal_array[HS_GENERAL_LED1].port,signal_array[HS_GENERAL_LED2].pin,HIGH);				//LED1 on

	Sleep(press_time_ms);

	SetOutput(signal_array[HS_TEL_ON_SIGNAL].port,signal_array[HS_TEL_ON_SIGNAL].pin,LOW);			//+12V off
	SetOutput(signal_array[HS_SELECT_TEL_ON_RXD].port,signal_array[HS_SELECT_TEL_ON_RXD].pin,HIGH); 	//TXD_A on TELON WCA
	//SetOutput(signal_array[HS_GENERAL_LED1].port,signal_array[HS_GENERAL_LED2].pin,LOW);				//LED1 off

	return result;
}