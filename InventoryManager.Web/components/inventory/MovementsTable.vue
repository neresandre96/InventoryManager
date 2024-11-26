<template>
    <v-data-table 
        :headers="movementHeaders" 
        :items="filteredMovements" 
        item-key="Id" 
        :sort-by="['date']"
        :sort-desc="[true]" 
        :loading="loadingMovements" 
        loading-text="Carregando... Aguarde um momento">
        
        <!-- Slot para exibição de movimentos -->
        <template v-slot:item.move="{ item }">
            <v-chip :color="item.move === 'Entrada' ? 'green' : 'red'" text-color="white">
                {{ item.move }}
            </v-chip>
        </template>
    </v-data-table>
</template>

<script>
export default {
    name: 'MovementsTable',
    props: {
        filteredMovements: {
            type: Array,
            required: true
        },
        movementHeaders: {
            type: Array,
            required: true
        },
        loadingMovements: {
            type: Boolean,
            default: false
        }
    }
};
</script>

<style scoped>
.v-chip {
    font-weight: bold;
}
</style>
