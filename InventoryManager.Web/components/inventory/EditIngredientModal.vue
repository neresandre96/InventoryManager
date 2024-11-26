<template>
  <v-dialog v-model="show" max-width="500px">
    <v-card>
      <v-card-title>Editar Ingrediente</v-card-title>
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field v-model="editedItem.name" label="Nome" :rules="[rules.required]"></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-select v-model="editedItem.unit" :items="units" label="Medida" outlined
                :rules="[rules.required]"></v-select>
            </v-col>
            <v-col cols="12">
              <v-text-field v-model="editedItem.minWeight" label="Peso mínimo" suffix="g"
                :rules="[rules.required, rules.positive]"></v-text-field>
            </v-col>
          </v-row>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="error" text @click="$emit('close')">Cancelar</v-btn>
        <v-btn color="primary" text @click="$emit('save')">Salvar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: 'EditIngredientModal',
  props: {
    show: {
      type: Boolean,
      default: false
    },
    editedItem: {
      type: Object,
      required: true
    },
    units: {
      type: Array,
      required: true
    },
    rules: {
      type: Object,
      required: true
    }
  }
}
</script>