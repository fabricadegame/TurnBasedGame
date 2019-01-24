/*
  Stockfish, a UCI chess playing engine derived from Glaurung 2.1
  Copyright (C) 2004-2008 Tord Romstad (Glaurung author)
  Copyright (C) 2008-2015 Marco Costalba, Joona Kiiski, Tord Romstad
  Copyright (C) 2015-2018 Marco Costalba, Joona Kiiski, Gary Linscott, Tord Romstad

  Stockfish is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  Stockfish is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

#ifndef FAIRY_CHESS_UCI_HPP_INCLUDED
#define FAIRY_CHESS_UCI_HPP_INCLUDED

#include <map>
#include <string>
#include <vector>

#include "fairy_chess_types.hpp"

namespace FairyChess
{
    class Position;
    
    namespace UCI {
        
        class Option;
        
        /// Custom comparator because UCI options should be case insensitive
        struct CaseInsensitiveLess {
            bool operator() (const std::string&, const std::string&) const;
        };
        
        /// Our options container is actually a std::map
        typedef std::map<std::string, Option, CaseInsensitiveLess> OptionsMap;
        
        /// Option class implements an option as defined by UCI protocol
        class Option {
            
            typedef void (*OnChange)(const Option&);
            
        public:
            Option(OnChange = nullptr);
            Option(bool v, OnChange = nullptr);
            Option(const char* v, OnChange = nullptr);
            Option(const char* v, const std::vector<std::string>& variants, OnChange = nullptr);
            Option(double v, int32_t minv, int32_t maxv, OnChange = nullptr);
            
            Option& operator=(const std::string&);
            void operator<<(const Option&);
            operator double() const;
            operator std::string() const;
            bool operator==(const char*);
            
        private:
            friend std::ostream& operator<<(std::ostream&, const OptionsMap&);
            
            std::string defaultValue, currentValue, type;
            int32_t min, max;
            std::vector<std::string> comboValues;
            size_t idx;
            OnChange on_change;
        };
        
        void init(OptionsMap&);
        std::string value(Value v);
        
        std::string square(Square s);
        void square(char* ret, Square s);
        
        std::string move(const Position& pos, Move m);
        void move(char* ret, const Position& pos, Move m);
        
        Move to_move(Position* pos, std::string& str);
        
    } // namespace UCI
    
    extern UCI::OptionsMap Options;
}


#endif // #ifndef UCI_H_INCLUDED