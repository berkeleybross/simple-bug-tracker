<template>
  <v-row align="center">
    <v-form ref="form" v-model="valid">
      <v-text-field
        v-model="title"
        :counter="100"
        :rules="titleRules"
        label="Title"
        required
      ></v-text-field>

      <v-textarea
        v-model="description"
        :rules="descriptionRules"
        :counter="1000"
        label="Description"
        required
      ></v-textarea>

      <v-select
        v-model="activeUser"
        :items="users"
        item-value="id"
        item-text="name"
        label="Active user"
        required
      ></v-select>

      <v-btn color="success" class="mr-4" @click="submit">
        Submit
      </v-btn>
    </v-form>
  </v-row>
</template>

<script>
export default {
  async asyncData({ app }) {
    return {
      valid: true,
      title: '',
      titleRules: [
        (v) => !!v || 'Title is required',
        (v) =>
          (v && v.length <= 100) || 'Title must be less than 100 characters',
      ],
      description: '',
      descriptionRules: [
        (v) =>
          !v ||
          v.length <= 1000 ||
          'Description must be less than 1000 characters',
      ],
      activeUser: null,
      users: [
        { id: null, name: '<no-one>' },
        ...(await app.$http.$get('api/users')),
      ],
    }
  },

  methods: {
    async submit() {
      if (!this.$refs.form.validate()) {
        return
      }

      await this.$http.$post('api/bugs', {
        title: this.title,
        description: this.description,
        activeUserId: this.activeUser,
      })

      this.$router.push({ name: 'index' })
    },
  },
}
</script>
