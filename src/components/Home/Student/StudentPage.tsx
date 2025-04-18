import { useState } from 'react';

function StudentPage() {
    const [selectedLink, setSelectedLink] = useState('home');
    return (
        <nav>
            <div onClick={() => { setSelectedLink("Home") }}></div>
            <div onClick={() => { setSelectedLink("MyCourses") }}></div>
            <div onClick={() => { setSelectedLink("MyModules") }}></div>
        </nav>
    );
}

export default StudentPage;