<template>
    <v-row justify="center" align="center" class="mb-4">
        <v-col cols="4">
            <v-btn depressed color="primary" @click="$emit('show-form', 'ingrediente')">
                <v-icon left>mdi-plus</v-icon> Registrar Ingredient
            </v-btn>
        </v-col>


        <v-col cols="2">
            <v-btn depressed color="primary" @click="$emit('show-form', 'entrada')">
                <v-icon left>mdi-inbox-arrow-up</v-icon> Entrada
            </v-btn>
        </v-col>

        <v-col cols="2">
            <v-btn depressed color="error" @click="$emit('show-form', 'saida')">
                <v-icon left>mdi-inbox-arrow-down</v-icon> Saída
            </v-btn>
        </v-col>


        <v-spacer />

        <v-col cols="3">
            <v-btn depressed color="blue-grey" class="btn-mkt-list custom-border" @click="$emit('show-shopping-list')">
                <v-icon left>mdi-format-list-checkbox</v-icon>Lista de compras
            </v-btn>
        </v-col>
    </v-row>
</template>
<script>
export default {
    name: 'ActionButtons'
}
</script>