<template>
    <div class="playback-request">
        <youtube-video 
            :data="data.video"
            :options="options"
        />
    </div>
</template>

<script>
    import YoutubeVideo from './YoutubeVideo.vue';

    export default {
        components: {
            "youtube-video": YoutubeVideo
        },
        props: {
            data: {
                type: Object,
                required: true
            },
            options: {
                type: Array,
                required: false,
                default: []
            }
        },
        mounted() {
            console.log("PlaybackRequest - mounted.");
            console.log(this.data);
        }
    }
</script>