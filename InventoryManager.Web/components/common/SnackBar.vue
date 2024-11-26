<template>
  <v-snackbar v-model="show" :color="success ? 'green' : 'red'" timeout="3000">
    {{ message }}
    <v-btn color="white" text @click="$emit('close')">
      Fechar
    </v-btn>
  </v-snackbar>
</template>

<script>
export default {
  name: 'SnackBar',
  props: {
    show: {
      type: Boolean,
      default: false
    },
    message: {
      type: String,
      default: ''
    },
    success: {
      type: Boolean,
      default: true
    }
  }
}
</script>
