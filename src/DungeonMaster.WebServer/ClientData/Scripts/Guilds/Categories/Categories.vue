<template>
    <div class="categories-container">
        <h1>Categories | <button class="btn btn-primary">Create</button></h1>

        <create-or-select-category-input name="category"></create-or-select-category-input>

        <hr />

        <div class="form-group row">
            <div class="col-8">
                <input type="button" class="btn btn-success" value="Save" />
            </div>
        </div>
        
    </div>
</template>

<script>
    import axios from 'axios';
    import CreateOrSelectCategoryInput from '../../Shared/Components/CreateOrSelectCategoryInput.vue';

    export default {
        components: {
            'create-or-select-category-input': CreateOrSelectCategoryInput
        },
        data() {
            return {
                socketCategoryChannels: []
            }
        },
        mounted() {
        },
        methods:
        {
        }
    }
</script>