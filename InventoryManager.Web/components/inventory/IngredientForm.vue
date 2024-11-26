<template>
  <v-row align="center">
    <v-row class="mr-2">
      <v-label>Ingrediente</v-label>
    </v-row>

    <v-col cols="3">
      <v-text-field v-model="form.name" label="Nome" :rules="[rules.required, rules.uniqueName]"></v-text-field>
    </v-col>

    <v-col cols="2">
      <v-select v-model="form.unit" :items="units" label="Medida" outlined :rules="[rules.required]"></v-select>
    </v-col>

    <v-col cols="2">
      <v-text-field v-model="form.minWeight" label="Peso mínimo" suffix="g"
        :rules="[rules.required, rules.positive]"></v-text-field>
    </v-col>

    <v-col cols="2">
      <v-text-field v-model="form.initialWeight" label="Peso inicial" suffix="g"
        :rules="[rules.positiveOrZero]"></v-text-field>
    </v-col>

    <v-spacer />

    <v-col cols="2" class="d-flex flex-column">
      <v-btn depressed x-small color="primary" class="mb-2" @click="submitForm">
        Confirmar
      </v-btn>

      <v-btn depressed x-small color="error" @click="$emit('cancel')">
        Cancelar
      </v-btn>
    </v-col>
  </v-row>
</template>

<script>
export default {
  name: 'IngredientForm',
  props: {
    units: {
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
        name: '',
        unit: '',
        minWeight: '',
        initialWeight: ''
      }
    }
  },
  methods: {
    submitForm() {
      if (!this.form.unit || !this.form.name || !this.form.minWeight) {
        this.$emit('show-error', 'Por favor, preencha todos os campos obrigatórios.');
        return;
      }

      const ingredientData = {
        name: this.form.name,
        minWeight: Number(this.form.minWeight),
        actualWeight: Number(this.form.initialWeight) || 0,
        measureUnit: Number(this.form.unit),
      }

      this.$emit('submit', ingredientData, this.resetForm);
    },
    resetForm() {
      this.form = {
        name: '',
        unit: '',
        minWeight: '',
        initialWeight: ''
      }
    }
  }
}
</script>
