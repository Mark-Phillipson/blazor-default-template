using AutoMapper;
using MSPApplication.Shared;
using MSPApplication.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSPApplication.Data
{
    public class MapperProfile: Profile
    {
		public MapperProfile()
		{
			CreateMap<AspNetUser, User>();
			// And others here as required
		}
    }
}
