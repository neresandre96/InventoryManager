<template>
  <v-container fluid fill-height>
    <v-row justify="center" align="center">

      <v-col cols="14" sm="8" md="10">
        <div class="pl-4">

          <action-buttons @show-form="showForm" @show-shopping-list="showShoppingList" />

          <!-- Formulário de Ingrediente -->
          <v-row v-if="currentForm === 'ingrediente'" align="center">
            <v-row class="mr-2">
              <v-label>Ingrediente</v-label>
            </v-row>

            <v-col cols="3">
              <v-text-field v-model="name" label="Nome" :rules="[rules.required, rules.uniqueName]"></v-text-field>
            </v-col>

            <v-col cols="2">
              <v-select v-model="unit" :items="units" label="Medida" outlined :rules="[rules.required]"></v-select>
            </v-col>

            <v-col cols="2">
              <v-text-field v-model="minWeight" label="Peso mínimo" suffix="g"
                :rules="[rules.required, rules.positive]"></v-text-field>
            </v-col>
            <v-col cols="2">
              <v-text-field v-model="initialWeight" label="Peso inicial" suffix="g"></v-text-field>
            </v-col>
            <v-spacer />

            <v-col cols="2" class="d-flex flex-column">

              <v-btn depressed x-small color="primary" class="mb-2" @click="submitIngredient()">
                Confirmar
              </v-btn>

              <v-btn depressed x-small color="error" @click="showForm(null)">
                Cancelar
              </v-btn>
            </v-col>
          </v-row>

          <!-- Formulário de Entrada -->
          <v-row v-if="currentForm === 'entrada'" align="center">
            <v-col cols="1">
              <v-label>Entrada</v-label>
            </v-col>
            <v-col cols="3">
              <v-autocomplete v-model="selectedIngredient" :items="ingredients" label="Ingrediente" item-text="name"
                item-value="id" return-object :rules="[rules.required]"></v-autocomplete>
            </v-col>
            <v-col cols="2">
              <v-text-field v-model="peso" label="Peso" suffix="g"
                :rules="[rules.required, rules.positive]"></v-text-field>
            </v-col>
            <v-col cols="2">
              <v-text-field v-model="custo" label="Custo" prefix="R$"
                :rules="[rules.required, rules.positive]"></v-text-field>
            </v-col>
            <v-spacer />
            <v-col cols="2">
              <v-btn depressed color="primary" @click="submitMovement('entrada')">Confirmar</v-btn>
            </v-col>
            <v-col cols="2">
              <v-btn depressed color="error" @click="showForm(null)">Cancelar</v-btn>
            </v-col>
          </v-row>

          <!-- Formulário de Saída -->
          <v-row v-if="currentForm === 'saida'" align="center">
            <v-col cols="1">
              <v-label>Saída</v-label>
            </v-col>
            <v-col cols="3">
              <v-autocomplete v-model="selectedIngredient" :items="ingredients" label="Ingrediente" item-text="name"
                item-value="id" return-object :rules="[rules.required]"></v-autocomplete>
            </v-col>
            <v-col cols="2">
              <v-text-field v-model="peso" label="Peso" suffix="g"
                :rules="[rules.required, rules.positive]"></v-text-field>
            </v-col>
            <v-spacer />
            <v-col cols="2">
              <v-btn depressed color="primary" @click="submitMovement('saida')">Confirmar</v-btn>
            </v-col>
            <v-col cols="2">
              <v-btn depressed color="error" @click="showForm(null)">Cancelar</v-btn>
            </v-col>
          </v-row>
        </div>
        <v-card>
          <v-tabs v-model="activeTab" background-color="primary" dark grow>
            <v-tab>Estoque</v-tab>
            <v-tab>Histórico</v-tab>
          </v-tabs>

          <v-tabs-items v-model="activeTab">

            <v-tab-item>
              <v-card-text>
                <v-text-field label="Buscar" v-model="searchQuery"></v-text-field>
                <v-data-table :headers="ingredientHeaders" :items="filteredIngredients" item-key="Id"
                  :loading="loadingIngredients" loading-text="Carregando... Aguarde um momento">
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
              </v-card-text>
            </v-tab-item>

            <v-tab-item>

              <v-card-text>
                <v-text-field label="Buscar" v-model="searchQuery"></v-text-field>
                <v-data-table :headers="movementHeaders" :items="filteredMovements" item-key="Id" :sort-by="['date']"
                  :sort-desc="[true]" :loading="loadingMovements" loading-text="Carregando... Aguarde um momento">
                  <template v-slot:item.move="{ item }">
                    <v-chip :color="item.move === 'Entrada' ? 'green' : 'red'" text-color="white">
                      {{ item.move }}
                    </v-chip>
                  </template>
                </v-data-table>
              </v-card-text>
              
            </v-tab-item>

          </v-tabs-items>

        </v-card>

        <snack-bar :show="snackbar.visible" :message="snackbar.message" :success="snackbar.success"
          @close="snackbar.visible = false" />

      </v-col>
    </v-row>

    <shopping-list-modal :show="showShoppingListDialog" :ingredients="filteredLowQuantityIngredients" @copy-list="copyList"
      @close="closeShoppingList" />

    <edit-ingredient-modal :show="showEditIngredientDialog" :edited-item="editedItem" :units="units" :rules="rules"
      @save="saveEdit" @close="closeEdit" />

  </v-container>

</template>

<script>
import axios from 'axios';
import ActionButtons from '@/components/inventory/ActionButtons.vue'
import ShoppingListModal from '~/components/inventory/ShoppingListModal.vue';
import EditIngredientModal from '~/components/inventory/EditIngredientModal.vue';
import SnackBar from '~/components/common/SnackBar.vue';

export default {
  middleware: 'auth',
  components: {
    ActionButtons,
    ShoppingListModal,
    EditIngredientModal,
    SnackBar
  },
  data() {
    return {
      rules: {
        required: v => !!v || 'Campo obrigatório.',
        positive: v => v > 0 || 'Valor deve numérico ser maior que zero.',
        uniqueName: v => !this.ingredients.some(i => i.name.toLowerCase() === v?.toLowerCase()) || 'Ingrediente já existe.'
      },
      ingredientHeaders: [
        { text: 'Ingrediente', value: 'name' },
        { text: 'Qntd', value: 'quantity' },
        { text: 'Qntd Mínima', value: 'minimumQuantity' },
        { text: 'Status', value: 'status' },
        { text: '', value: 'actions', sortable: false }
      ],
      movementHeaders: [
        { text: 'Data', value: 'date' },
        { text: 'Movimentação', value: 'move' },
        { text: 'Ingrediente', value: 'name' },
        { text: 'Qntd', value: 'quantity' },
        { text: 'Custo', value: 'cost' },
      ],
      units: [
        { text: 'g', value: '0' },
        { text: 'kg', value: '1' },
        { text: 'ml', value: '2' },
        { text: 'L', value: '3' },
      ],
      ingredients: [],
      movements: [],
      activeTab: 0,
      
      loadingIngredients: true,
      loadingMovements: true,
      searchQuery: '',
      currentForm: null,
      selectedIngredient: null,
      peso: '',
      custo: '',
      name: '',
      minWeight: '',
      unit: '',
      initialWeight: '',
      showShoppingListDialog: false,
      showEditIngredientDialog: false,
      editedItem: {
        id: '',
        name: '',
        unit: '',
        minWeight: ''
      },
      snackbar: {
        visible: false,
        message: '',
        success: false
      }
    };
  },
  computed: {
    filteredIngredients() {
      return this.ingredients.filter(ingredient => {
        return ingredient.name.toLowerCase().includes(this.searchQuery.toLowerCase());
      });
    },
    filteredMovements() {
      return this.movements.filter(movement => {
        return movement.name.toLowerCase().includes(this.searchQuery.toLowerCase());
      });
    },
    filteredLowQuantityIngredients() {
      return this.ingredients.filter(ingredient => ingredient.status === "Fora de estoque");
    }
  },
  mounted() {
    this.fetchIngredients();
    this.fetchMovements();
  },
  methods: {
    editIngredient(item) {

      const unitMapping = {
        "g": "0",
        "kg": "1",
        "ml": "2",
        "l": "3"
      };


      const mappedUnit = unitMapping[item.measureUnit];

      console.log("Unit: " + item.measureUnit, "Mapped Unit:", mappedUnit);

      this.editedItem = {
        id: item.id,
        name: item.name,
        unit: mappedUnit,
        minWeight: item.minimumQuantity.replace(/\D/g, ''),
      }
      this.showEditIngredientDialog = true
    },

    closeEdit() {
      this.showEditIngredientDialog = false
      this.editedItem = {
        id: '',
        name: '',
        unit: '',
        minWeight: ''
      }
    },
    closeShoppingList() {
      this.showShoppingListDialog = false

    },

    async saveEdit() {
      try {
        const response = await axios.put(`http://localhost:5165/api/Ingredient/${this.editedItem.id}`, {
          name: this.editedItem.name,
          minWeight: Number(this.editedItem.minWeight),
          measureUnit: Number(this.editedItem.unit)
        }, { withCredentials: true })

        this.showSnackbar('Ingrediente atualizado com sucesso!', true)
        this.closeEdit()
        this.fetchIngredients()
      } catch (error) {
        this.showSnackbar('Erro ao atualizar ingrediente: ' + error.message, false)
      }
    },
    async deleteIngredient(item) {

      try {
        const response = await axios.delete(`http://localhost:5165/api/Ingredient/${item.id}`, { withCredentials: true });

        this.fetchIngredients();
        this.snackbar.message = 'Ingrediente excluído com sucesso!';
        this.snackbar.success = true;
        this.snackbar.visible = true;
      } catch (error) {
        this.snackbar.message = 'Erro ao excluir ingrediente. ' + error.message;
        this.snackbar.success = false;
        this.snackbar.visible = true;
      }

    },
    async fetchIngredients() {
      try {
        const response = await axios.get('http://localhost:5165/api/Ingredient', { withCredentials: true });

        const unitMapping = {
          0: "g",
          1: "kg",
          2: "ml",
          3: "l",
        };

        this.ingredients = response.data.map(ingredient => ({
          id: ingredient.id,
          name: ingredient.name,
          measureUnit: unitMapping[ingredient.measureUnit],
          quantity: ingredient.actualWeight + unitMapping[ingredient.measureUnit],
          minimumQuantity: ingredient.minWeight + unitMapping[ingredient.measureUnit],
          status: ingredient.isBelowMin === false ? "Em estoque" : "Fora de estoque",
        }));

        this.loadingIngredients = false;
      } catch (error) {
        console.error('Erro ao buscar os dados:', error);
      }
    },
    async fetchMovements() {
      try {
        const response = await axios.get('http://localhost:5165/api/Movement', { withCredentials: true });

        this.movements = response.data.map(movement => ({
          id: movement.id,
          date: new Intl.DateTimeFormat("pt-BR", {
            day: "2-digit",
            month: "2-digit",
            year: "numeric",
            hour: "2-digit",
            minute: "2-digit",
            second: "2-digit",
          }).format(new Date(movement.createdAt)),
          name: movement.name,
          quantity: movement.quantity,
          cost: movement.price === 0 ? "*" : movement.price,
          move: movement.isEntry === false ? "Saída" : "Entrada",
        }));

        this.loadingMovements = false;
      } catch (error) {
        console.error('Erro ao buscar os dados:', error);
      }
    },
    showForm(form) {
      this.currentForm = form;
      this.peso = '';
      this.custo = '';
      this.selectedIngredient = null;
    },
    showShoppingList() {
      this.showShoppingListDialog = true;
    },
    async submitMovement(type) {
      if (!this.selectedIngredient || !this.peso || (!this.custo && type === 'entrada')) {
        this.showSnackbar('Por favor, preencha todos os campos.', false);
        return;
      }

      try {
        const movementData = {
          ingredientId: this.selectedIngredient.id,
          name: this.selectedIngredient.name,
          isEntry: type === 'entrada',
          quantity: parseFloat(this.peso) || 0,
          price: parseFloat(this.custo) || 0
        };

        console.log('Dados do movimento:', movementData);

        const response = await axios.post('http://localhost:5165/api/Movement', movementData, { withCredentials: true });

        this.showSnackbar('Movimento realizado com sucesso!', true);
        this.fetchIngredients();
        this.fetchMovements();
        this.showForm(null);
      } catch (error) {
        this.showSnackbar('Erro ao realizar o movimento. ' + error.message, false);
      }
    },
    async copyList() {
      const listText = this.filteredLowQuantityIngredients
        .map(ingredient => `${ingredient.name} - Comprar: ${ingredient.minimumQuantity}`)
        .join('\n');

      await navigator.clipboard.writeText(listText);

      this.showSnackbar('Lista copiada com sucesso!', true);
      this.showShoppingListDialog = false;
    },
    async submitIngredient() {
      if (!this.unit || !this.name || !this.minWeight) {
        this.showSnackbar('Por favor, preencha todos os campos obrigatórios.', false);
        return;
      }

      try {
        const ingredientData = {
          name: this.name,
          minWeight: Number(this.minWeight),
          actualWeight: Number(this.initialWeight),
          measureUnit: Number(this.unit)
        };

        console.log('Dados do ingrediente:', ingredientData);

        const response = await axios.post('http://localhost:5165/api/Ingredient', ingredientData, { withCredentials: true });

        this.showSnackbar('Ingrediente cadastrado com sucesso!', true);
        this.fetchIngredients();
        this.showForm(null);
      } catch (error) {
        this.showSnackbar('Erro ao realizar o cadastro. ' + error.message, false);
      }
    },
    showSnackbar(message, success) {
      this.snackbar.message = message;
      this.snackbar.success = success;
      this.snackbar.visible = true;
    }
  },
};
</script>


<style scoped>
.custom-border {
  border: 2px solid #000;
  border-radius: 4px;
}
</style>