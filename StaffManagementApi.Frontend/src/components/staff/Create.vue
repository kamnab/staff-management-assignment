<script setup>
import { ref } from 'vue'
import { createStaff } from '../../services/staffService'
import { useRouter } from 'vue-router'
const router = useRouter()

const staff = ref({
    staffId: '',
    fullName: '',
    gender: '',
    birthday: ''
})

const isSubmitting = ref(false)
const message = ref('')

const handleSubmit = async () => {
    message.value = ''
    isSubmitting.value = true

    try {
        const success = await createStaff(staff.value)
        if (success) {
            message.value = '✅ Staff created successfully!'
            // Clear the form
            staff.value = { staffId: '', fullName: '', gender: '', birthday: '' }
        } else {
            message.value = '❌ Failed to create staff.'
        }
    } catch (error) {
        console.error('Create staff failed:', error)
        message.value = '❌ Error occurred while creating staff.'
    } finally {
        isSubmitting.value = false
    }
}

</script>

<template>
    <div style="max-width: 400px; margin: auto;">
        <h2>Create New Staff</h2>

        <form @submit.prevent="handleSubmit" style="display: flex; flex-direction: column; gap: 10px;">
            <div>
                <label>Staff ID</label><br />
                <input v-model="staff.staffId" type="text" required placeholder="Enter Staff ID" />
            </div>

            <div>
                <label>Full Name</label><br />
                <input v-model="staff.fullName" type="text" required placeholder="Enter Full Name" />
            </div>

            <div>
                <label>Gender</label><br />
                <select v-model="staff.gender" required>
                    <option disabled value="">Select Gender</option>
                    <option value="1">Male</option>
                    <option value="2">Female</option>
                </select>
            </div>

            <div>
                <label>Birthday</label><br />
                <input v-model="staff.birthday" type="date" required />
            </div>

            <div style="display: flex; gap: 8px; margin-top: 8px;">
                <button type="submit" :disabled="isSubmitting" style="padding: 6px 12px;">
                    {{ isSubmitting ? 'Saving...' : 'Create' }}
                </button>

                <button type="button" @click="() => router.push('/')" style="padding: 6px 12px;">
                    Back
                </button>
            </div>

            <p v-if="message">{{ message }}</p>
        </form>
    </div>
</template>
