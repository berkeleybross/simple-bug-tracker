<template>
  <v-form ref="form" v-model="valid">
    <v-container>
      <v-row>
        <v-col cols="12" md="4">
          <v-text-field
            v-model="name"
            :rules="nameRules"
            :counter="61"
            label="Name"
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
  async asyncData({ app, params }) {
    const user = await app.$http.$get(['/api/users/{id}', { id: params.id }])
    return {
      valid: true,
      id: params.id,
      name: user.name,
      nameRules: [
        (v) => !!v || 'Name is required',
        (v) => v.length <= 61 || 'Name must be less than 61 characters',
      ],
    }
  },

  methods: {
    async submit() {
      if (!this.$refs.form.validate()) {
        return
      }

      await this.$http.$put(['api/users/{id}', { id: this.id }], {
        name: this.name,
      })

      this.$router.push({ name: 'users' })
    },
  },
}
</script>
