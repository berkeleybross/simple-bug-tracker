<template>
  <v-form ref="form" v-model="valid">
    <v-container>
      <v-row>
        <v-col cols="12" md="4">
          <v-text-field
            v-model="firstname"
            :rules="nameRules"
            :counter="30"
            label="First name"
            required
          ></v-text-field>
        </v-col>

        <v-col cols="12" md="4">
          <v-text-field
            v-model="lastname"
            :rules="nameRules"
            :counter="30"
            label="Last name"
            required
          ></v-text-field>
        </v-col>
      </v-row>

      <v-row>
        <v-btn color="success" class="mr-4" @click="submit">
          Submit
        </v-btn>
      </v-row>
    </v-container>
  </v-form>
</template>

<script>
export default {
  data: () => ({
    valid: false,
    firstname: '',
    lastname: '',
    nameRules: [
      (v) => !!v || 'Name is required',
      (v) => v.length <= 30 || 'Name must be less than 30 characters',
    ],
  }),

  methods: {
    async submit() {
      if (!this.$refs.form.validate()) {
        return
      }

      await this.$http.$post('api/users', {
        name: this.firstname + ' ' + this.lastname,
      })

      this.$router.push({ name: 'users' })
    },
  },
}
</script>
