﻿#pragma once

#include "il2cpp-config.h"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif

#include <stdint.h>

#include "AssemblyU2DCSharp_Gomoku_NoneRuleInputUI_UIData_Su2489631484.h"

// VP`1<System.Int32>
struct VP_1_t2450154454;
// VP`1<Gomoku.NoneRule.ChoosePieceAdapter/UIData>
struct VP_1_t1895160151;




#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// Gomoku.NoneRule.SetPieceUI/UIData
struct  UIData_t1129684821  : public Sub_t2489631484
{
public:
	// VP`1<System.Int32> Gomoku.NoneRule.SetPieceUI/UIData::square
	VP_1_t2450154454 * ___square_5;
	// VP`1<Gomoku.NoneRule.ChoosePieceAdapter/UIData> Gomoku.NoneRule.SetPieceUI/UIData::choosePieceAdapter
	VP_1_t1895160151 * ___choosePieceAdapter_6;

public:
	inline static int32_t get_offset_of_square_5() { return static_cast<int32_t>(offsetof(UIData_t1129684821, ___square_5)); }
	inline VP_1_t2450154454 * get_square_5() const { return ___square_5; }
	inline VP_1_t2450154454 ** get_address_of_square_5() { return &___square_5; }
	inline void set_square_5(VP_1_t2450154454 * value)
	{
		___square_5 = value;
		Il2CppCodeGenWriteBarrier(&___square_5, value);
	}

	inline static int32_t get_offset_of_choosePieceAdapter_6() { return static_cast<int32_t>(offsetof(UIData_t1129684821, ___choosePieceAdapter_6)); }
	inline VP_1_t1895160151 * get_choosePieceAdapter_6() const { return ___choosePieceAdapter_6; }
	inline VP_1_t1895160151 ** get_address_of_choosePieceAdapter_6() { return &___choosePieceAdapter_6; }
	inline void set_choosePieceAdapter_6(VP_1_t1895160151 * value)
	{
		___choosePieceAdapter_6 = value;
		Il2CppCodeGenWriteBarrier(&___choosePieceAdapter_6, value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif