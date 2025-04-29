import { Route, Routes } from "react-router-dom";
import Evaluation from "./Evaluation/Evaluation";
import Module from "./Module/Module";
import { useEffect, useState } from "react";
import { useApi } from "../../../customs/useApi";
import { useParams } from "react-router-dom";

interface AIModuleDto {
    Id: string;
    Title: string;
    Content: string;
    Status: boolean;
}

function Modules() {
    const { callApi } = useApi();
    const { id } = useParams<{ id: string }>();
    const [modules, setModules] = useState<AIModuleDto[] | null>(null);

    useEffect(() => {
        callApi("GET", `course/${id}/modules`).then((response) => {
            return response.json();
        }).then((data) => {
            setModules(data);
        });
    }, []);

    return (
    <div className="modules-list">
        {modules && modules.map((module) => (
            <Routes>
                <Route path={`/modules/${module.Id}`} element={<Module />} />
                <Route path={`/eval/${module.Id}`} element={<Evaluation />} />
            </Routes>
        ))}
    </div>
    );
}

export default Modules;