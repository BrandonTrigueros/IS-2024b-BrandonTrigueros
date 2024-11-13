<template>
  <div class="d-flex justify-content-center align-items-center vh-100">
    <div class="card p-4 shadow" style="max-width: 400px; width: 100%">
      <h3 class="text-center">Formulario de creación de países</h3>

      <form @submit.prevent="guardarPais">

        <div class="form-group">
          <label for="nombre">Nombre:</label>
          <input 
            v-model="datosFormulario.nombre" 
            type="text" 
            id="nombre"
            class="form-control" 
            required />
        </div>

        <div class="form-group">
          <label for="continente">Continente:</label>
          <select 
            v-model="datosFormulario.continente" 
            id="continente" 
            required
            class="form-control"
            >
            <option value="" disabled>Seleccione un continente</option>
            <option>África</option>
            <option>Asia</option>
            <option>Europa</option>
            <option>América</option>
            <option>Oceanía</option>
            <option>Antártida</option>
          </select>
        </div>

        <div class="form-group">
          <label for="idioma">Idioma:</label>
          <input 
            v-model="datosFormulario.idioma" 
            type="text" 
            id="idioma"
            class="form-control" 
            required 
          />
        </div>

        <div>
          <button type="submit" class="btn btn-success btn-block">
            Guardar
          </button>
        </div>

      </form>

    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  data() {
    return {
      datosFormulario: { nombre: "", continente: "", idioma: "" },
    };
  },
  methods: {
    guardarPais() {
      console.log("Datos a guardar:", this.datosFormulario);
      console.log("Nombre:", this.datosFormulario.nombre);
      console.log("Continente:", this.datosFormulario.continente);
      console.log("Idioma:", this.datosFormulario.idioma);
      axios
        .post("https://localhost:7261/api/Paises", {
          nombre: this.datosFormulario.nombre,
          continente: this.datosFormulario.continente,
          idioma: this.datosFormulario.idioma,
        })
        .then(function (response) {
          console.log(response);
          window.location.href = "/";
        })
        .catch(function (error) {
          console.log(error);
        });
    },
  },
};
</script>

<style lang="scss" scoped></style>