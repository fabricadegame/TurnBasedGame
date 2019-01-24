﻿#pragma once

#include "il2cpp-config.h"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif

#include <stdint.h>

#include "AssemblyU2DCSharp_UIBehavior_1_gen780466895.h"

// UnityEngine.UI.Button
struct Button_t2872111280;
// UnityEngine.UI.Text
struct Text_t356221433;
// AdvancedCoroutines.Routine
struct Routine_t2502590640;
// GameManager.Match.RoundRobin.RequestNewRoundRobinStateAskUI/UIData
struct UIData_t1602907382;
// Server
struct Server_t2724320767;




#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// GameManager.Match.RoundRobin.RequestNewRoundRobinAskBtnCancelUI
struct  RequestNewRoundRobinAskBtnCancelUI_t2779371412  : public UIBehavior_1_t780466895
{
public:
	// UnityEngine.UI.Button GameManager.Match.RoundRobin.RequestNewRoundRobinAskBtnCancelUI::btnCancel
	Button_t2872111280 * ___btnCancel_6;
	// UnityEngine.UI.Text GameManager.Match.RoundRobin.RequestNewRoundRobinAskBtnCancelUI::tvCancel
	Text_t356221433 * ___tvCancel_7;
	// AdvancedCoroutines.Routine GameManager.Match.RoundRobin.RequestNewRoundRobinAskBtnCancelUI::wait
	Routine_t2502590640 * ___wait_8;
	// GameManager.Match.RoundRobin.RequestNewRoundRobinStateAskUI/UIData GameManager.Match.RoundRobin.RequestNewRoundRobinAskBtnCancelUI::requestNewRoundRobinStateAskUIData
	UIData_t1602907382 * ___requestNewRoundRobinStateAskUIData_9;
	// Server GameManager.Match.RoundRobin.RequestNewRoundRobinAskBtnCancelUI::server
	Server_t2724320767 * ___server_10;

public:
	inline static int32_t get_offset_of_btnCancel_6() { return static_cast<int32_t>(offsetof(RequestNewRoundRobinAskBtnCancelUI_t2779371412, ___btnCancel_6)); }
	inline Button_t2872111280 * get_btnCancel_6() const { return ___btnCancel_6; }
	inline Button_t2872111280 ** get_address_of_btnCancel_6() { return &___btnCancel_6; }
	inline void set_btnCancel_6(Button_t2872111280 * value)
	{
		___btnCancel_6 = value;
		Il2CppCodeGenWriteBarrier(&___btnCancel_6, value);
	}

	inline static int32_t get_offset_of_tvCancel_7() { return static_cast<int32_t>(offsetof(RequestNewRoundRobinAskBtnCancelUI_t2779371412, ___tvCancel_7)); }
	inline Text_t356221433 * get_tvCancel_7() const { return ___tvCancel_7; }
	inline Text_t356221433 ** get_address_of_tvCancel_7() { return &___tvCancel_7; }
	inline void set_tvCancel_7(Text_t356221433 * value)
	{
		___tvCancel_7 = value;
		Il2CppCodeGenWriteBarrier(&___tvCancel_7, value);
	}

	inline static int32_t get_offset_of_wait_8() { return static_cast<int32_t>(offsetof(RequestNewRoundRobinAskBtnCancelUI_t2779371412, ___wait_8)); }
	inline Routine_t2502590640 * get_wait_8() const { return ___wait_8; }
	inline Routine_t2502590640 ** get_address_of_wait_8() { return &___wait_8; }
	inline void set_wait_8(Routine_t2502590640 * value)
	{
		___wait_8 = value;
		Il2CppCodeGenWriteBarrier(&___wait_8, value);
	}

	inline static int32_t get_offset_of_requestNewRoundRobinStateAskUIData_9() { return static_cast<int32_t>(offsetof(RequestNewRoundRobinAskBtnCancelUI_t2779371412, ___requestNewRoundRobinStateAskUIData_9)); }
	inline UIData_t1602907382 * get_requestNewRoundRobinStateAskUIData_9() const { return ___requestNewRoundRobinStateAskUIData_9; }
	inline UIData_t1602907382 ** get_address_of_requestNewRoundRobinStateAskUIData_9() { return &___requestNewRoundRobinStateAskUIData_9; }
	inline void set_requestNewRoundRobinStateAskUIData_9(UIData_t1602907382 * value)
	{
		___requestNewRoundRobinStateAskUIData_9 = value;
		Il2CppCodeGenWriteBarrier(&___requestNewRoundRobinStateAskUIData_9, value);
	}

	inline static int32_t get_offset_of_server_10() { return static_cast<int32_t>(offsetof(RequestNewRoundRobinAskBtnCancelUI_t2779371412, ___server_10)); }
	inline Server_t2724320767 * get_server_10() const { return ___server_10; }
	inline Server_t2724320767 ** get_address_of_server_10() { return &___server_10; }
	inline void set_server_10(Server_t2724320767 * value)
	{
		___server_10 = value;
		Il2CppCodeGenWriteBarrier(&___server_10, value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif