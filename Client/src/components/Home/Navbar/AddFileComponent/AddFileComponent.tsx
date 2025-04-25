import { useState } from "react";

function AddFileComponent() {
    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const selectedFile = event.target.files?.[0];

        if (selectedFile) {
            const reader = new FileReader();
            reader.onload = (e) => {
                const data = {
                    title: selectedFile.name.split(".")[0],
                    fileContent: new Uint8Array(e.target?.result as ArrayBuffer),
                    createdAt: new Date().toISOString(),
                }
                console.log("Selected file:", data);
                // fetch('/upload', {
                //     method: 'POST',
                //     body: JSON.stringify(data),
                // })
                //     .then(response => response.json())
                //     .then(data => console.log('File uploaded successfully:', data))
                //     .catch(error => console.error('Error uploading file:', error));
            }
            reader.readAsArrayBuffer(selectedFile);
        }
    };

    return (
        <label htmlFor="file-upload" className="custom-file-upload">
            <input id="file-upload" type="file" className="add-course-button" onChange={(e) => handleFileChange(e)} />
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className="feather feather-plus-circle"><circle cx="12" cy="12" r="10"></circle><line x1="12" y1="8" x2="12" y2="16"></line><line x1="8" y1="12" x2="16" y2="12"></line></svg>
            <span className="add-course-text">Add Course</span>
        </label>
    )
}

export default AddFileComponent;