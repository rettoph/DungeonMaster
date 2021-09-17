<template>
    <div class="music-container">
        <div class="row">
            <div class="col-6">
                <div class="row">
                    <div class="col-12">
                        <h1>Now Playing</h1>
                    </div>
                </div>

                <div class="row">
                    <youtube-video v-if="info.nowPlaying != null"
                                   :data="info.nowPlaying"
                                   :options="NOW_PLAYING_OPTIONS"
                                   @optionClicked="handleResultOptionClicked" />
                </div>

                <hr />

                <div class="row">
                    <div class="col-12">
                        <h1>Playlist</h1>
                    </div>
                </div>

                <div class="row">
                    <youtube-video v-if="info.queue != null"
                                   v-for="video in info.queue"
                                   :data="video"
                                   :options="PLAYLIST_OPTIONS"
                                   @optionClicked="handleResultOptionClicked" />
                </div>
            </div>

            <div class="col-6">
                <div class="row">
                    <div class="col-12">
                        <h1>Search</h1>
                    </div>
                </div>

                <div class="row">
                    <div class="col-10">
                        <input v-model="query" id="query-input" name="query-input" type="text" class="form-control" placeholder="Query">
                    </div>
                    <div class="col-2">
                        <button class="btn btn-primary" @click="handleSearchClicked">Search</button>
                    </div>
                </div>

                <div class="row">
                    <youtube-video 
                        v-for="result in results" 
                        :data="result" 
                        :options="RESULT_OPTIONS"
                        @optionClicked="handleResultOptionClicked" />
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import YoutubeVideo from '../../Shared/Components/YoutubeVideo.vue';

    const RESULT_OPTIONS = {
        ADD_TO_QUEUE: "Add to Queue",
        PLAY_NEXT: "Play Next",
        PLAY_NOW: "Play Now"
    }

    const PLAYLIST_OPTIONS = {
        REMOVE: "Remove"
    }

    const NOW_PLAYING_OPTIONS = {
        SKIP: "Skip"
    }

    export default {
        components: {
            'youtube-video': YoutubeVideo
        },
        data() {
            return {
                info: {},
                query: localStorage.getItem("query"),
                lastQuery: "",
                results: JSON.parse(localStorage.getItem("results") ?? "[]"),
                RESULT_OPTIONS: [ RESULT_OPTIONS.ADD_TO_QUEUE, RESULT_OPTIONS.PLAY_NEXT, RESULT_OPTIONS.PLAY_NOW ],
                PLAYLIST_OPTIONS: [ PLAYLIST_OPTIONS.REMOVE ],
                NOW_PLAYING_OPTIONS: [ NOW_PLAYING_OPTIONS.SKIP ],
            }
        },
        mounted() {
            this.updateInfo();
        },
        methods: {
            /* Helper Methods */
            trySearch() {
                if (this.query == "")
                    return;
                if (this.query == this.lastQuery)
                    return;

                this.Search();

                this.lastQuery = this.query;
            },
            Search() {
                localStorage.setItem("query", this.query);

                axios.get(`/api/youtube/search?query=${encodeURIComponent(this.query)}`)
                    .then(r => r.data)
                    .then(this.handleSearchResponse);
            },
            updateInfo() {
                axios.get(`/api/guilds/${window.CurrentGuildId}/music`)
                    .then(r => r.data)
                    .then(this.handleUpdateInfoResponse);
            },

            /* Event Handlers */
            handleSearchClicked() {
                this.trySearch();
            },
            handleResultOptionClicked(event) {
                switch (event.option) {
                    case RESULT_OPTIONS.ADD_TO_QUEUE:
                        axios.get(`/api/guilds/${window.CurrentGuildId}/music/enqueue?videoId=${event.video.id}`)
                            .then(r => r.data)
                            .then(this.handleResultOptionClickedResponse);
                        break;
                    case RESULT_OPTIONS.PLAY_NEXT:
                        axios.get(`/api/guilds/${window.CurrentGuildId}/music/play-next?videoId=${event.video.id}`)
                            .then(r => r.data)
                            .then(this.handleResultOptionClickedResponse);
                        break;
                    case RESULT_OPTIONS.PLAY_NOW:
                        axios.get(`/api/guilds/${window.CurrentGuildId}/music/play?videoId=${event.video.id}`)
                            .then(r => r.data)
                            .then(this.handleResultOptionClickedResponse);
                        break;
                }
            },

            /* Axios Response Handlers */
            handleSearchResponse(data) {
                this.results = data;

                localStorage.setItem("results", JSON.stringify(data));
            },

            handleResultOptionClickedResponse(data) {
                this.info = data;
            },

            handleUpdateInfoResponse(data) {
                this.info = data;

                setTimeout(this.updateInfo, 10000, this);
            }
        }
    }
</script>