<template>
  <v-dialog v-model="show" max-width="500px">
    <v-card>
      <v-card-title>
        Lista de Compras
      </v-card-title>
      <v-card-text>
        <v-list>
          <v-list-item v-for="ingredient in ingredients" :key="ingredient.id">
            <v-list-item-content>
              <v-list-item-title>{{ ingredient.name }}</v-list-item-title>
              <v-list-item-subtitle>Em estoque: {{ ingredient.quantity }}</v-list-item-subtitle>
              <v-list-item-subtitle>Compra mínima: {{ ingredient.minimumQuantity }}</v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-list>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="primary" text @click="$emit('copy-list')">Copiar</v-btn>
        <v-btn color="error" text @click="$emit('close')">Fechar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: 'ShoppingListModal',
  props: {
    show: {
      type: Boolean,
      default: false
    },
    ingredients: {
      type: Array,
      default: () => []
    }
  }
}
</script>