﻿#pragma once

#include "il2cpp-config.h"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif

#include <stdint.h>

#include "AssemblyU2DCSharp_DataIdentity543951486.h"

// NetData`1<Makruk.DefaultMakruk>
struct NetData_1_t776981995;




#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// Makruk.DefaultMakrukIdentity
struct  DefaultMakrukIdentity_t4113714712  : public DataIdentity_t543951486
{
public:
	// System.Boolean Makruk.DefaultMakrukIdentity::chess960
	bool ___chess960_17;
	// NetData`1<Makruk.DefaultMakruk> Makruk.DefaultMakrukIdentity::netData
	NetData_1_t776981995 * ___netData_18;

public:
	inline static int32_t get_offset_of_chess960_17() { return static_cast<int32_t>(offsetof(DefaultMakrukIdentity_t4113714712, ___chess960_17)); }
	inline bool get_chess960_17() const { return ___chess960_17; }
	inline bool* get_address_of_chess960_17() { return &___chess960_17; }
	inline void set_chess960_17(bool value)
	{
		___chess960_17 = value;
	}

	inline static int32_t get_offset_of_netData_18() { return static_cast<int32_t>(offsetof(DefaultMakrukIdentity_t4113714712, ___netData_18)); }
	inline NetData_1_t776981995 * get_netData_18() const { return ___netData_18; }
	inline NetData_1_t776981995 ** get_address_of_netData_18() { return &___netData_18; }
	inline void set_netData_18(NetData_1_t776981995 * value)
	{
		___netData_18 = value;
		Il2CppCodeGenWriteBarrier(&___netData_18, value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif