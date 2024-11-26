<template>
    <v-data-table 
      :headers="ingredientHeaders" 
      :items="filteredIngredients" 
      item-key="Id"
      :loading="loadingIngredients" 
      loading-text="Carregando... Aguarde um momento">
      
      <template v-slot:item.status="{ item }">
        <v-chip :color="item.status === 'Em estoque' ? 'green' : 'red'" text-color="white">
          {{ item.status }}
        </v-chip>
      </template>
  
      <template v-slot:item.actions="{ item }">
        <v-btn icon small class="mr-2" @click="editIngredient(item)">
          <v-icon small>mdi-pencil</v-icon>
        </v-btn>
  
        <v-btn icon small @click="deleteIngredient(item)">
          <v-icon small>mdi-delete</v-icon>
        </v-btn>
      </template>
  
    </v-data-table>
  </template>
  
  <script>
  export default {
    name: 'IngredientsTable',
    props: {
      filteredIngredients: {
        type: Array,
        required: true
      },
      ingredientHeaders: {
        type: Array,
        required: true
      },
      loadingIngredients: {
        type: Boolean,
        default: false
      }
    },
    emits: ['edit', 'delete'],
    methods: {
      editIngredient(item) {
        this.$emit('edit', item);
      },
      deleteIngredient(item) {
        this.$emit('delete', item);
      },
    }
  };
  </script>
  
  <style scoped>
  .v-chip {
    font-weight: bold;
  }
  </style>
  