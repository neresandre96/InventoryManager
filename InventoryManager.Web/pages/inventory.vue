<template>
  <v-container fluid fill-height>
    <v-row justify="center" align="center">

      <v-col cols="14" sm="8" md="10">
        <div class="pl-4">

          <action-buttons @show-form="showForm" @show-shopping-list="showShoppingList" />

          <ingredient-form v-if="currentForm === 'ingrediente'" :units="units" :rules="rules"
            @submit="handleIngredientSubmit" @cancel="showForm(null)" @show-error="showSnackbar($event, false)" />

          <entry-form v-if="currentForm === 'entrada'" :ingredients="ingredients" :rules="rules"
            @submit="handleEntrySubmit" @cancel="showForm(null)" @show-error="showSnackbar($event, false)" />

          <exit-form v-if="currentForm === 'saida'" :ingredients="ingredients" :rules="rules" @submit="handleExitSubmit"
            @cancel="showForm(null)" @show-error="showSnackbar($event, false)" />

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

                <ingredients-table :ingredientHeaders="ingredientHeaders" :filteredIngredients="filteredIngredients"
                  :loadingIngredients="loadingIngredients" @edit="editIngredient" @delete="deleteIngredient" />

              </v-card-text>
            </v-tab-item>

            <v-tab-item>

              <v-card-text>
                <v-text-field label="Buscar" v-model="searchQuery"></v-text-field>

                <movements-table :movementHeaders="movementHeaders" :filteredMovements="filteredMovements"
                  :loadingMovements="loadingMovements" />

              </v-card-text>
            </v-tab-item>
          </v-tabs-items>
        </v-card>

        <snack-bar :show="snackbar.visible" :message="snackbar.message" :success="snackbar.success"
          @close="snackbar.visible = false" />

      </v-col>
    </v-row>

    <shopping-list-modal :show="showShoppingListDialog" :ingredients="filteredLowQuantityIngredients"
      @copy-list="copyList" @close="closeShoppingList" />

    <edit-ingredient-modal :show="showEditIngredientDialog" :edited-item="editedItem" :units="units" :rules="rules"
      @save="saveEdit" @close="closeEdit" />

  </v-container>

</template>

<script>
import { inventoryService } from '~/services/api.service';
import ActionButtons from '@/components/inventory/ActionButtons.vue';
import ShoppingListModal from '~/components/inventory/ShoppingListModal.vue';
import EditIngredientModal from '~/components/inventory/EditIngredientModal.vue';
import SnackBar from '~/components/common/SnackBar.vue';
import IngredientForm from '~/components/inventory/IngredientForm.vue';
import EntryForm from '~/components/inventory/EntryForm.vue';
import ExitForm from '~/components/inventory/ExitForm.vue';
import IngredientsTable from '~/components/inventory/IngredientsTable.vue';
import MovementsTable from '~/components/inventory/MovementsTable.vue';

export default {
  middleware: 'auth',
  components: {
    ActionButtons,
    ShoppingListModal,
    EditIngredientModal,
    SnackBar,
    IngredientForm,
    EntryForm,
    ExitForm,
    IngredientsTable,
    MovementsTable
  },
  data() {
    return {
      rules: {
        required: v => !!v || 'Campo obrigatório.',
        positive: v => v > 0 || 'Valor deve numérico ser maior que zero.',
        positiveOrZero: v => v >= 0 || 'Valor deve ser numérico ou zero.',
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
    async saveEdit() {
      try {
        await inventoryService.updateIngredient(this.editedItem.id, {
          name: this.editedItem.name,
          minWeight: Number(this.editedItem.minWeight),
          measureUnit: Number(this.editedItem.unit)
        })

        this.showSnackbar('Ingrediente atualizado com sucesso!', true)
        this.closeEdit()
        this.fetchIngredients()
      } catch (error) {
        this.showSnackbar('Erro ao atualizar ingrediente: ' + error.message, false)
      }
    },
    async deleteIngredient(item) {

      try {
        await inventoryService.deleteIngredient(item.id);

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
        const response = await inventoryService.getIngredients();

        const unitMapping = {
          0: "g",
          1: "kg",
          2: "ml",
          3: "l",
        };

        this.ingredients = response.map(ingredient => ({
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
        const response = await inventoryService.getMovements();

        this.movements = response.map(movement => ({
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
    async handleEntrySubmit(movementData, resetForm) {
      try {
        await inventoryService.createMovement(movementData)
        this.showSnackbar('Movimento realizado com sucesso!', true)
        this.fetchIngredients();
        this.fetchMovements();
        this.showForm(null);
        resetForm()
      } catch (error) {
        this.showSnackbar('Erro ao realizar o movimento. ' + error.message, false)
      }
    },
    async handleExitSubmit(movementData) {
      try {
        await inventoryService.createMovement(movementData);

        this.showSnackbar('Movimento realizado com sucesso!', true);
        this.fetchIngredients();
        this.fetchMovements();
        this.showForm(null);
        resetForm()
      } catch (error) {
        this.showSnackbar('Erro ao realizar o movimento. ' + error.message, false)
      }
    },
    async handleIngredientSubmit(ingredientData, resetForm) {
      try {
        await inventoryService.createIngredient(ingredientData);

        this.showSnackbar('Ingrediente cadastrado com sucesso!', true);
        this.fetchIngredients();
        this.showForm(null);
        resetForm();
      } catch (error) {
        this.showSnackbar('Erro ao realizar o cadastro. ' + error.message, false)
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
    showShoppingList() {
      this.showShoppingListDialog = true;
    },
    showForm(form) {
      this.currentForm = form;
      this.peso = '';
      this.custo = '';
      this.selectedIngredient = null;
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