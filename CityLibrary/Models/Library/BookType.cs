using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLibrary.Models.Library
{
    public enum BookType
    {
        NotSet = 0,
        Available, 
        Borrowed,
        All
    }
}