//
//  xiangqi_jni.hpp
//  TestXiangqi
//
//  Created by Viet Tho on 3/7/18.
//  Copyright © 2018 Viet Tho. All rights reserved.
//

#ifndef xiangqi_jni_hpp
#define xiangqi_jni_hpp

#include <stdio.h>
#include <string.h>
#include <stdint.h>

namespace Xiangqi
{
    // neu ko co extern "C" thi o C# ko nhan
    extern "C"
    {
        
        extern char xiangqi_bookPath[1024];
        
        bool xiangqi_setBookPath(const char* newBookPath);
        
        void xiangqi_initCore();
        
        ///////////////////////////////////////////////////////////////////////////////
        ///////////////////// Core ////////////////////
        ///////////////////////////////////////////////////////////////////////////////

        int32_t xiangqi_makePositionByFen(const char* strFen, uint8_t* &outRet);
        
        void xiangqi_printPosition(uint8_t* positionBytes, int32_t length, bool canCorrect);
        
        void xiangqi_printPositionFen(uint8_t* positionBytes, int32_t length, bool canCorrect);

        int32_t xiangqi_isGameFinish(uint8_t* positionBytes, int32_t length, bool canCorrect);
        
        uint32_t xiangqi_letComputerThink(uint8_t* positionBytes, int32_t length, bool canCorrect, int32_t depth, int32_t lngLimitTime, bool useBook, int32_t pickBestMove);
        
        void xiangqi_printMove(uint32_t move);
        
        bool xiangqi_isLegalMove(uint8_t* positionBytes, int32_t length, bool canCorrect, uint32_t move);

        int32_t xiangqi_doMove(uint8_t* positionBytes, int32_t length, bool canCorrect, uint32_t move, uint8_t* &outRet);

        int32_t xiangqi_getLegalMoves(uint8_t* positionBytes, int32_t length, bool canCorrect, uint8_t* &outLegalMoves);
        
        ///////////////////////////////////////////////////////////////////////////////
        ///////////////////// Evaluate Position ////////////////////
        ///////////////////////////////////////////////////////////////////////////////
        
        struct EvaluateResult
        {
            int32_t material = 0;
            int32_t advisorShape = 0;
            int32_t stringHold = 0;
            int32_t rookMobility = 0;
            int32_t knightTrap = 0;
        };
        
        void evaluatePosition(uint8_t* positionBytes, int32_t length, bool canCorrect, int32_t depth, int32_t lngLimitTime, bool useBook, int32_t chosenType, int32_t differentScore);
    }

}

#endif /* xiangqi_jni_hpp */