using AutoMapper;
using Discord;
using Discord.WebSocket;
using DungeonMaster.WebServer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.AutoMapper
{
    public class SocketCategoryChannelProfile : Profile
    {
        public SocketCategoryChannelProfile()
        {
            this.CreateMap<IChannel, ChannelViewModel>()
                .ForMember(d => d.Id, s => s.MapFrom(x => x.Id))
                .ForMember(d => d.Name, s => s.MapFrom(x => x.Name));
        }
    }
}
