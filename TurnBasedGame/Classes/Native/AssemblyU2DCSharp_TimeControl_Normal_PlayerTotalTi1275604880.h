﻿#pragma once

#include "il2cpp-config.h"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif

#include <stdint.h>

#include "AssemblyU2DCSharp_DataIdentity543951486.h"

// NetData`1<TimeControl.Normal.PlayerTotalTime>
struct NetData_1_t3271659831;




#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// TimeControl.Normal.PlayerTotalTimeIdentity
struct  PlayerTotalTimeIdentity_t1275604880  : public DataIdentity_t543951486
{
public:
	// System.Int32 TimeControl.Normal.PlayerTotalTimeIdentity::playerIndex
	int32_t ___playerIndex_17;
	// System.Single TimeControl.Normal.PlayerTotalTimeIdentity::serverTime
	float ___serverTime_18;
	// System.Single TimeControl.Normal.PlayerTotalTimeIdentity::clientTime
	float ___clientTime_19;
	// NetData`1<TimeControl.Normal.PlayerTotalTime> TimeControl.Normal.PlayerTotalTimeIdentity::netData
	NetData_1_t3271659831 * ___netData_20;

public:
	inline static int32_t get_offset_of_playerIndex_17() { return static_cast<int32_t>(offsetof(PlayerTotalTimeIdentity_t1275604880, ___playerIndex_17)); }
	inline int32_t get_playerIndex_17() const { return ___playerIndex_17; }
	inline int32_t* get_address_of_playerIndex_17() { return &___playerIndex_17; }
	inline void set_playerIndex_17(int32_t value)
	{
		___playerIndex_17 = value;
	}

	inline static int32_t get_offset_of_serverTime_18() { return static_cast<int32_t>(offsetof(PlayerTotalTimeIdentity_t1275604880, ___serverTime_18)); }
	inline float get_serverTime_18() const { return ___serverTime_18; }
	inline float* get_address_of_serverTime_18() { return &___serverTime_18; }
	inline void set_serverTime_18(float value)
	{
		___serverTime_18 = value;
	}

	inline static int32_t get_offset_of_clientTime_19() { return static_cast<int32_t>(offsetof(PlayerTotalTimeIdentity_t1275604880, ___clientTime_19)); }
	inline float get_clientTime_19() const { return ___clientTime_19; }
	inline float* get_address_of_clientTime_19() { return &___clientTime_19; }
	inline void set_clientTime_19(float value)
	{
		___clientTime_19 = value;
	}

	inline static int32_t get_offset_of_netData_20() { return static_cast<int32_t>(offsetof(PlayerTotalTimeIdentity_t1275604880, ___netData_20)); }
	inline NetData_1_t3271659831 * get_netData_20() const { return ___netData_20; }
	inline NetData_1_t3271659831 ** get_address_of_netData_20() { return &___netData_20; }
	inline void set_netData_20(NetData_1_t3271659831 * value)
	{
		___netData_20 = value;
		Il2CppCodeGenWriteBarrier(&___netData_20, value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif