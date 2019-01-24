﻿#pragma once

#include "il2cpp-config.h"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif

#include <stdint.h>

#include "AssemblyU2DCSharp_UIBehavior_1_gen1384789723.h"

// UnityEngine.UI.Image
struct Image_t2042527209;
// UnityEngine.UI.Text
struct Text_t356221433;
// Chess.UseRuleInputUI/UIData
struct UIData_t1395244174;




#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// Chess.UseRule.BtnChosenMoveUI
struct  BtnChosenMoveUI_t3355485285  : public UIBehavior_1_t1384789723
{
public:
	// UnityEngine.UI.Image Chess.UseRule.BtnChosenMoveUI::imgPromotion
	Image_t2042527209 * ___imgPromotion_6;
	// UnityEngine.UI.Text Chess.UseRule.BtnChosenMoveUI::tvMoveType
	Text_t356221433 * ___tvMoveType_7;
	// UnityEngine.UI.Text Chess.UseRule.BtnChosenMoveUI::tvMove
	Text_t356221433 * ___tvMove_8;
	// Chess.UseRuleInputUI/UIData Chess.UseRule.BtnChosenMoveUI::useRuleInputUIData
	UIData_t1395244174 * ___useRuleInputUIData_9;

public:
	inline static int32_t get_offset_of_imgPromotion_6() { return static_cast<int32_t>(offsetof(BtnChosenMoveUI_t3355485285, ___imgPromotion_6)); }
	inline Image_t2042527209 * get_imgPromotion_6() const { return ___imgPromotion_6; }
	inline Image_t2042527209 ** get_address_of_imgPromotion_6() { return &___imgPromotion_6; }
	inline void set_imgPromotion_6(Image_t2042527209 * value)
	{
		___imgPromotion_6 = value;
		Il2CppCodeGenWriteBarrier(&___imgPromotion_6, value);
	}

	inline static int32_t get_offset_of_tvMoveType_7() { return static_cast<int32_t>(offsetof(BtnChosenMoveUI_t3355485285, ___tvMoveType_7)); }
	inline Text_t356221433 * get_tvMoveType_7() const { return ___tvMoveType_7; }
	inline Text_t356221433 ** get_address_of_tvMoveType_7() { return &___tvMoveType_7; }
	inline void set_tvMoveType_7(Text_t356221433 * value)
	{
		___tvMoveType_7 = value;
		Il2CppCodeGenWriteBarrier(&___tvMoveType_7, value);
	}

	inline static int32_t get_offset_of_tvMove_8() { return static_cast<int32_t>(offsetof(BtnChosenMoveUI_t3355485285, ___tvMove_8)); }
	inline Text_t356221433 * get_tvMove_8() const { return ___tvMove_8; }
	inline Text_t356221433 ** get_address_of_tvMove_8() { return &___tvMove_8; }
	inline void set_tvMove_8(Text_t356221433 * value)
	{
		___tvMove_8 = value;
		Il2CppCodeGenWriteBarrier(&___tvMove_8, value);
	}

	inline static int32_t get_offset_of_useRuleInputUIData_9() { return static_cast<int32_t>(offsetof(BtnChosenMoveUI_t3355485285, ___useRuleInputUIData_9)); }
	inline UIData_t1395244174 * get_useRuleInputUIData_9() const { return ___useRuleInputUIData_9; }
	inline UIData_t1395244174 ** get_address_of_useRuleInputUIData_9() { return &___useRuleInputUIData_9; }
	inline void set_useRuleInputUIData_9(UIData_t1395244174 * value)
	{
		___useRuleInputUIData_9 = value;
		Il2CppCodeGenWriteBarrier(&___useRuleInputUIData_9, value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif