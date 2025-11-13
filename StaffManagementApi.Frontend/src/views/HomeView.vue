<script setup>
import { ref, onMounted } from 'vue'
import { getStaffById, getAllStaffs, exportToExcel } from '../services/staffService';
import { useRouter } from 'vue-router';
const router = useRouter();

const staffs = ref([]) // to store staff list
const searchQuery = ref('') // to store input search 

const fetchStaffs = async () => {
    try {
        const data = await getAllStaffs()
        staffs.value = data || []
    } catch (error) {
        console.error('Failed to fetch staffs:', error)
    }
}

const searchStaffs = async () => {
    try {
        // If input is empty, reload all staffs
        if (!searchQuery.value.trim()) {
            await fetchStaffs()
            return
        }
        // otherwise, filtered the data
        const data = await getAllStaffs()
        staffs.value = data.filter(staff =>
            staff.fullName.toLowerCase().includes(searchQuery.value.toLowerCase())
        )
    } catch (error) {
        console.error('Search failed:', error)
    }
}

const handleExport = async () => {
    try {
        // Pass searchQuery to backend
        await exportToExcel(searchQuery.value.trim())
    } catch (error) {
        console.error('Export failed:', error)
    }
}

onMounted(fetchStaffs)
</script>

<template>
    <h1>Staff Management Assignment</h1>
    <div style="margin-bottom: 1rem;">
        <input v-model="searchQuery" placeholder="Search by name..." style="padding: 6px; width: 200px;" />
        <button @click="searchStaffs" style="padding: 6px 12px; margin-left: 4px;">Search</button>
        <button @click="handleExport" style="padding: 6px 12px; margin-left: 4px;">Export</button>
    </div>

    <div>
        <button type="button" @click="() => router.push('/new-staff')" style="padding: 6px 12px;">
            Add New
        </button>
        <table border="1" cellspacing="0" cellpadding="6">
            <thead>
                <tr>
                    <th>Staff ID</th>
                    <th>Full Name</th>
                    <th>Gender</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="staff in staffs" :key="staff.staffId">
                    <td>{{ staff.staffId }}</td>
                    <td>{{ staff.fullName }}</td>
                    <td>{{ staff.gender === 1 ? 'Male' : staff.gender === 2 ? 'Female' : 'Unknown' }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>
