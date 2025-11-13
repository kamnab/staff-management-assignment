const getStaffById = async (id) => {
    try {
        const res = await fetch(`${import.meta.env.VITE_BACKEND_API_URL}/api/staffs/${id}`);

        if (!res.ok) throw new Error(`HTTP error! Status: ${res.status}`);

        const data = await res.json();
        console.log(data);
        const { staffId, fullName, gender, birthday } = data;
        return { staffId, fullName, gender, birthday };

    } catch (error) {
        console.error('API call failed:', error);
    }

    return null
};

const getAllStaffs = async () => {
    try {
        const res = await fetch(`${import.meta.env.VITE_BACKEND_API_URL}/api/staffs/search`);

        if (!res.ok) throw new Error(`HTTP error! Status: ${res.status}`);

        const data = await res.json();
        console.log(data);
        return data;

    } catch (error) {
        console.error('API call failed:', error);
    }

    return null
};

const exportToExcel = async (fullName = '') => {
    try {
        // Build query string safely
        const query = fullName ? `?fullName=${encodeURIComponent(fullName)}` : ''
        const res = await fetch(`${import.meta.env.VITE_BACKEND_API_URL}/api/staffs/export/excel${query}`)

        if (!res.ok) throw new Error(`HTTP error! Status: ${res.status}`)

        // Since API returns a file, read as blob
        const blob = await res.blob()

        // Create a download link
        const link = document.createElement('a')
        link.href = URL.createObjectURL(blob)
        link.download = 'staffs.xlsx'
        link.click()
        URL.revokeObjectURL(link.href)
    } catch (error) {
        console.error('Export failed:', error)
    }
}

const createStaff = async (staff) => {
    console.log(staff);
    try {
        const res = await fetch(`${import.meta.env.VITE_BACKEND_API_URL}/api/staffs`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json', // it is important to specific the header for asp.net web api to bind to [FromBody] attribute
            },
            body: JSON.stringify(staff),
        })

        if (!res.ok) throw new Error(`HTTP error! Status: ${res.status}`)
        return true
    } catch (error) {
        console.error('API call failed:', error)
        return false
    }
}

export { getStaffById, getAllStaffs, exportToExcel, createStaff }