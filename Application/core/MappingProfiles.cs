using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Model;

using Common.DTOs;
using Common.DTOs.User;
using Common.DTOs.LastStationID;
using Common.DTOs.DataTrack;

namespace Application.core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<DataTrack, DataTrackDTO>()
                .ForMember(dest => dest.LastStationID, opt => opt.MapFrom(src => src.LastStationID))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDataDto
                {
                    DisplayName = src.User.DisplayName,
                    UserName = src.User.UserName,
                    // tambahkan properti lainnya sesuai kebutuhan
                }));
            //.ForMember(dest => dest.DataTrackCheckings, opt => opt.MapFrom(src => src.DataTrackCheckings));

            CreateMap<DataTrack, DetailDataTrackDto>()
                .ForMember(dest => dest.LastStationID, opt => opt.MapFrom(src => src.LastStationID))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDataDto
                {
                    DisplayName = src.User.DisplayName,
                    UserName = src.User.UserName,
                    // tambahkan properti lainnya sesuai kebutuhan
                }))
                .ForMember(dest => dest.DataTrackCheckings, opt => opt.MapFrom(src => src.DataTrackCheckings));

            CreateMap<DataLine, DataLineDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                    .ForMember(dest => dest.LineName, opt => opt.MapFrom(src => src.LineName))
                    .ForMember(dest => dest.isDeleted, opt => opt.MapFrom(src => src.isDeleted));

            CreateMap<LastStationID, LastStationIDDTO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.StationID, opt => opt.MapFrom(src => src.StationID))
                 .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                 .ForMember(dest => dest.DataLine, opt => opt.MapFrom(src => new DataLineDTO
                 {
                     Id = src.DataLine.Id,
                     LineId = src.DataLine.LineId,
                     LineName = src.DataLine.LineName,
                     isDeleted = src.DataLine.isDeleted
                 }));
                
CreateMap<DataTrackChecking, DataTrackCheckingDTO>()
    .ForMember(dest => dest.ParameterCheck, opt => opt.MapFrom(src => src.ParameterCheck))
    .ForMember(dest => dest.ImageDataChecks, opt => opt.MapFrom(src => src.ImageDataChecks));


            CreateMap<ImageDataCheck, ImageDataCheckDTO>();
            CreateMap<ParameterCheck, ParameterCheckDTO>();
            CreateMap<DataTrackChecking, DataTrackCheckingDTO>()
                .ForMember(dest => dest.ImageDataChecks, opt => opt.MapFrom(src => src.ImageDataChecks));
            // CreateMap<ImageDataCheck, ImageDataCheckDTO>()
            //     .ForMember(dest => dest.DataTrackCheckingId, opt => opt.MapFrom(src => src.DataTrackCheckingId))
            //     .ForMember(dest => dest.DataTrackChecking, opt => opt.MapFrom(src => src.DataTrackChecking));

            CreateMap<SelectOption, SelectOptionDTO>();
            CreateMap<DataContrplType, DataContrplTypeDTO>();
            CreateMap<SComboBoxOption, SComboBoxOptionDTO>();
            CreateMap<WorkOrder, WorkOrderDto>()
             .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDataDto
             {
                 DisplayName = src.User.DisplayName,
                 UserName = src.User.UserName,
                 // tambahkan properti lainnya sesuai kebutuhan
             }));

            CreateMap<DataReference, DataReferenceDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RefereceName, opt => opt.MapFrom(src => src.RefereceName))
                .ForMember(dest => dest.StationID, opt => opt.MapFrom(src => src.StationID)) // LineID dipetakan ke StationID
                .ForMember(dest => dest.isDeleted, opt => opt.MapFrom(src => src.isDeleted))
                .ForMember(dest => dest.LastStation, opt => opt.MapFrom(src => src.LastStationID)); // DataLine dipetakan ke LastStation



        }
    }
}