<template>
    <div class="youtube-video">
        <div class="thumbnail-container">
            <img class="thumbnail" :src="data.snippet.thumbnails.medium.url" />
            <span class="duration">{{durationToString(data.contentDetails.duration)}}</span>
        </div>
        
        <span class="title">{{data.snippet.title}}</span>

        <div class="options-container" v-if="options.length > 0">
            <span class="ellipsis">•••</span>

            <div class="options">
                <div v-for="option in options" class="option" @click="handleOptionClicked(option)">{{option}}</div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
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
        methods:
        {
            durationToString(duration) {
                var match = duration.match(/PT(\d+H)?(\d+M)?(\d+S)?/);

                match = match.slice(1).map(function (x) {
                    if (x != null) {
                        return x.replace(/\D/, '');
                    }
                });

                var hours = (parseInt(match[0]) || 0).toString();
                var minutes = (parseInt(match[1]) || 0).toString();
                var seconds = (parseInt(match[2]) || 0).toString();

                if (hours != '0')
                    return `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}:${seconds.padStart(2, '0')}`;
                else
                    return `${minutes.padStart(2, '0')}:${seconds.padStart(2, '0')}`;
            },

            handleOptionClicked(option) {
                this.$emit('optionClicked', {
                    option: option,
                    video: this.data
                });
            }
        }
    }
</script>

<style lang="scss">
    @import '../../../Styles/variables.scss';

    .youtube-video {
        display: flex;
        margin: 10px 0;
    }

    .youtube-video .thumbnail-container {
        position: relative;
    }

    .youtube-video .thumbnail {
        width: 150px;
    }

    .youtube-video .duration {
        position: absolute;
        bottom: 0;
        right: 0;
        padding: 3px 7px;
        background:  rgba(0, 0, 0, 0.3);
    }

    .youtube-video .title {
        padding: 15px;
    }

    .youtube-video .options-container {
        position: relative;
        margin-left: auto;
    }

    .youtube-video .options-container .ellipsis {
        padding: 3px 10px;
        font-size: 20px;
        color: $discord-color-1;
        cursor: pointer;
        border-radius: 5px;

        transition: background-color 0.3s cubic-bezier(.25,.8,.25,1);
    }

    .youtube-video .options-container:hover .ellipsis {
        background-color: $discord-color-3;
    }

    .youtube-video .options {
        display: none;
        position: absolute;
        top: 25px;
        right: 15px;
        background: $discord-color-6;
        background: $discord-color-5;
        box-shadow: $shadow-1;
        white-space: nowrap;
        border-radius: 5px;
        z-index: 20;

        -webkit-touch-callout: none; /* iOS Safari */
        -webkit-user-select: none; /* Safari */
         -khtml-user-select: none; /* Konqueror HTML */
           -moz-user-select: none; /* Old versions of Firefox */
            -ms-user-select: none; /* Internet Explorer/Edge */
                user-select: none; /* Non-prefixed version, currently
                                      supported by Chrome, Edge, Opera and Firefox */
    }

    .youtube-video .options-container:hover .options {
        display: block;
    }

    .youtube-video .option {
        padding: 5px 10px;
        cursor: pointer;

        transition: background-color 0.3s cubic-bezier(.25,.8,.25,1);
    }

    .youtube-video .option:hover {
        background: $discord-color-3;
    }
</style>