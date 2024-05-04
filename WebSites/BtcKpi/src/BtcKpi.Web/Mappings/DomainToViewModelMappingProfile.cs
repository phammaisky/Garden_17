using AutoMapper;
using BtcKpi.Model;
using BtcKpi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BtcKpi.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Category,CategoryViewModel>();
            Mapper.CreateMap<Gadget, GadgetViewModel>();
        }
    }
}
