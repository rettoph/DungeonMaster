<template>
    <div class="form-group row">
        <label for="text1" class="col-4 col-form-label">{{label}}</label>
        <div class="col-8">
            <select :name="name" :id="name" @click="handleClick" class="form-control dropdown-toggle dropdown" :aria-describedby="name + 'HelpBlock'">
                <option v-for="category in categories"
                        :value="category.id">
                    {{category.name}}
                </option>
            </select>

            <span :id="name + 'HelpBlock'" class="form-text text-muted">{{help}}</span>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import ChannelType from '../Enums/ChannelType.js';

    export default {
        props: {
            name: {
                type: String,
                required: true
            },
            label: {
                type: String,
                default: "Category"
            },
            help: {
                type: String,
                default: "Select a category."
            },
            withCategories: {
                type: Array,
                default: null
            }
        },
        data() {
            return {
                categories: []
            }
        },
        mounted() {
            if (this.withCategories == null) {
                axios.get(`/api/guilds/${window.CurrentGuildId}/channels?type=${ChannelType.Category}`)
                    .then(r => r.data)
                    .then(this.handleGetSocketCategoryChannels);
            }
            else {
                this.categories = this.withCategories;
            }
        },
        methods: {
            handleGetSocketCategoryChannels(categories) {
                this.categories = categories;
            },
            handleClick(e) {
                this.$emit('click', this.categories);
            }
        }
    }
</script>