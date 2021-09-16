<template>
    <div class="category-selector">
        <div v-if="categories.length == 0">
            <create-category-input
                :name="name + '[new]'"
            ></create-category-input>
        </div>

        <div v-if="categories.length > 0">

            <!-- Create New -->
            <div class="radio-container row" @click="handleNewInputClicked">
                <input type="radio" :id="name + '-new'" :name="name + '[type]'" value="new" ref="radioNew" checked  class="col-1" />

                <div class="col-11">
                    <create-category-input
                        :name="name + '[new]'"
                    ></create-category-input>
                </div>
            </div>

            <!-- Select Existing -->
            <div class="radio-container row" @click="handleExistingInputClicked">
                <input type="radio" :id="name + '-existing'" :name="name + '[type]'" value="existing" ref="radioExisting" class="col-1" />

                <div class="col-11">
                    <select-category-input 
                        :name="name + '[existing]'"
                        :with-categories="categories"
                        label="Existing Category"
                        help="Select a pre-existing category within your discord server."
                    ></select-category-input>
                </div>
            </div>
        </div>

    </div>
</template>

<script>
    import axios from 'axios';
    import SelectCategoryInput from './SelectCategoryInput.vue';
    import CreateCategoryInput from './CreateCategoryInput.vue';
    import ChannelType from '../Enums/ChannelType.js';

    export default {
        components: {
            "select-category-input": SelectCategoryInput,
            "create-category-input": CreateCategoryInput
        },
        props: {
            name: {
                type: String,
                required: true
            }
        },
        data() {
            return {
                categories: []
            }
        },
        mounted() {
            axios.get(`/api/guilds/${window.CurrentGuildId}/channels?type=${ChannelType.Category}`)
                .then(r => r.data)
                .then(this.handleGetSocketCategoryChannels);
        },
        methods: {
            handleNewInputClicked(e) {
                this.$refs['radioNew'].checked = true;
            },
            handleExistingInputClicked(e) {
                this.$refs['radioExisting'].checked = true;
            },
            handleGetSocketCategoryChannels(data) {
                this.categories = data;
            }
        }
    }
</script>