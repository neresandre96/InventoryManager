<template>
  <v-row align="center">
    <v-col cols="1">
      <v-label>Saída</v-label>
    </v-col>

    <v-col cols="3">
      <v-autocomplete 
        v-model="form.selectedIngredient" 
        :items="ingredients" 
        label="Ingrediente" 
        item-text="name"
        item-value="id" 
        return-object 
        :rules="[rules.required]"
      ></v-autocomplete>
    </v-col>

    <v-col cols="2">
      <v-text-field 
        v-model="form.weight" 
        label="Peso" 
        suffix="g"
        :rules="[rules.required, rules.positive]"
      ></v-text-field>
    </v-col>

    <v-spacer />

    <v-col cols="2">
      <v-btn 
        depressed 
        color="primary" 
        @click="submitForm"
      >
        Confirmar
      </v-btn>
    </v-col>

    <v-col cols="2">
      <v-btn 
        depressed 
        color="error" 
        @click="$emit('cancel')"
      >
        Cancelar
      </v-btn>
    </v-col>
  </v-row>
</template>

<script>
export default {
  name: 'ExitForm',
  props: {
    ingredients: {
      type: Array,
      required: true
    },
    rules: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      form: {
        selectedIngredient: null,
        weight: ''
      }
    }
  },
  methods: {
    submitForm() {
      if (!this.form.selectedIngredient || !this.form.weight) {
        this.$emit('show-error', 'Por favor, preencha todos os campos.');
        return;
      }

      const movementData = {
        ingredientId: this.form.selectedIngredient.id,
        name: this.form.selectedIngredient.name,
        isEntry: false,
        quantity: parseFloat(this.form.weight) || 0,
        price: 0
      }

      this.$emit('submit', movementData, this.resetForm());
    },
    resetForm() {
      this.form = {
        selectedIngredient: null,
        weight: ''
      }
    }
  }
}
</script>
