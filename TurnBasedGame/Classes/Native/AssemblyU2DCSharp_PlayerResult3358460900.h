﻿#pragma once

#include "il2cpp-config.h"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif

#include <stdint.h>

#include "AssemblyU2DCSharp_Data3569509720.h"

// VP`1<System.Int32>
struct VP_1_t2450154454;
// VP`1<System.Single>
struct VP_1_t2454786938;




#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// PlayerResult
struct  PlayerResult_t3358460900  : public Data_t3569509720
{
public:
	// VP`1<System.Int32> PlayerResult::playerIndex
	VP_1_t2450154454 * ___playerIndex_5;
	// VP`1<System.Single> PlayerResult::score
	VP_1_t2454786938 * ___score_6;

public:
	inline static int32_t get_offset_of_playerIndex_5() { return static_cast<int32_t>(offsetof(PlayerResult_t3358460900, ___playerIndex_5)); }
	inline VP_1_t2450154454 * get_playerIndex_5() const { return ___playerIndex_5; }
	inline VP_1_t2450154454 ** get_address_of_playerIndex_5() { return &___playerIndex_5; }
	inline void set_playerIndex_5(VP_1_t2450154454 * value)
	{
		___playerIndex_5 = value;
		Il2CppCodeGenWriteBarrier(&___playerIndex_5, value);
	}

	inline static int32_t get_offset_of_score_6() { return static_cast<int32_t>(offsetof(PlayerResult_t3358460900, ___score_6)); }
	inline VP_1_t2454786938 * get_score_6() const { return ___score_6; }
	inline VP_1_t2454786938 ** get_address_of_score_6() { return &___score_6; }
	inline void set_score_6(VP_1_t2454786938 * value)
	{
		___score_6 = value;
		Il2CppCodeGenWriteBarrier(&___score_6, value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif