using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonMaster.Library.Structs
{
    public struct PlaybackRequest
    {
        public Guid Id { get; }
        public CancellationTokenSource CancellationTokenSource { get; }
        public Video Video { get; }

        public PlaybackRequest(Video video) : this()
        {
            this.Id = Guid.NewGuid();
            this.CancellationTokenSource = new CancellationTokenSource();
            this.Video = video;
        }

        public override string ToString()
        {
            return $"PlaybackRequest({this.Id}) {{ Video('{this.Video.Snippet.Title}')<{this.Video.Id}> }}";
        }
    }
}
