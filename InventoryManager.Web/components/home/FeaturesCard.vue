<template>
    <v-col cols="12" md="6">
        <v-card outlined height="100%">
            <v-card-title>{{ title }}</v-card-title>
            <v-card-text>
                {{ message }}
            </v-card-text>
        </v-card>
    </v-col>
</template>

<script>
export default {
    name: 'FeaturesCard',
    props: {
        title: {
            type: String,
            default: ''
        },
        message: {
            type: String,
            default: ''
        }
    }
}
</script>