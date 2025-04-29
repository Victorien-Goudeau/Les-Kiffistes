import ReactMarkdown from "react-markdown";
import "./Module.css";
import { useEffect, useState } from "react";
import { Link, Outlet, Route, Routes, useParams } from "react-router-dom";
import { useApi } from "../../../../customs/useApi";

function Module() {
    const [markdownContent, setMarkdownContent] = useState<string>("");
    const { id } = useParams<{ id: string }>();

    const { callApi } = useApi();

    useEffect(() => {
        callApi("GET", `Course/${id}/modules`).then((response) => {
            return response.json();
        }).then((data) => {
            console.log(data);
        }
        ).catch((error) => {
            console.error("Erreur lors de la récupération des modules :", error);
        });
        fetch("/README.md")
            .then((response) => response.text())
            .then((text) => setMarkdownContent(text))
            .catch((error) => console.error("Erreur lors du chargement du fichier Markdown :", error));
    }, []);

    return (
        <div className="modules">
            <div className="sidebar">
                <div className="modules-list">
                    <h4>Modules</h4>
                    <div>Module 1</div>
                    <div>Module 2</div>
                    <div>Module 3</div>
                    <div>Module 4</div>
                    <div>Module 5</div>
                    <div>Module 6</div>
                    <div>Module 7</div>
                    <div>Module 8</div>
                </div>
            </div>
            <div className="modules-content">
                <h1>Modules</h1>
                <ReactMarkdown>{markdownContent}</ReactMarkdown>
                <Link to="/home/modules/eval" className="eval-link">
                    <div className="eval-button">
                        Start evaluation
                    </div>
                </Link>
            </div>
            {/* <Routes>
                <Route path="eval" element={<Evaluation />} />
            </Routes> */}
        </div>
    );
}

export default Module;