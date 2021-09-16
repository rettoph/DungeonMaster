<template>
    <div class="form-group row">
        <label for="text1" class="col-4 col-form-label">New Category</label>
        <span class="col-8">
            <input type="text" :name="name" class="form-control" :aria-describedby="name + 'HelpBlock'" />

            <span :id="name + 'HelpBlock'" class="form-text text-muted">Create a brand new category within your discord server.</span>
        </span>
    </div>
</template>

<script>
    export default {
        props: {
            name: {
                type: String,
                required: true
            }
        }
    }
</script>