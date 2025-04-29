import { useApi } from "../../../../customs/useApi";

function AddFileComponent() {
    const { callApi } = useApi();
    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        console.log("File selected:", event.target.files);
        const selectedFile = event.target.files?.[0];

        if (selectedFile) {
            const reader = new FileReader();
            reader.onload = (e) => {
                const data = {
                    title: selectedFile.name.split(".")[0],
                    fileContent: (e.target?.result as string).split(",")[1],
                    createdAt: new Date().toISOString(),
                }
                callApi("POST", "Course/add", JSON.stringify(data))
                    .then((response) => {
                        if (response.status === 200) {
                            return response.json();
                        } else {
                            throw new Error("File upload failed");
                        }
                    })
                    .then((data) => {
                        console.log("File uploaded successfully:", data);
                        console.log("ID ouaiche :", data.id);

                        callApi("POST", "quiz", JSON.stringify(data.id)).then((response) => {
                            if (response.status === 200) {
                                return response.json();
                            } else {
                                throw new Error("Quiz creation failed");
                            }
                        })
                            .then((data) => {
                                console.log("Quiz created successfully:", data);
                                event.target.files = null; // Clear the file input
                                window.location.href = `/home/eval/${data.courseId}`;
                            });

                    })
                    .catch((error) => {
                        console.error("Error:", error);
                    });
            }
            reader.readAsDataURL(selectedFile);
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