// src/pages/Remediation.tsx
import { useLocation, Navigate } from "react-router-dom";
import { useState } from "react";
import "./RemediationModules.css";
import Navbar from "../../Navbar/Navbar";

export interface RemediationModule {
    label: string;
    lesson: string;
    question: {
        prompt: string;
        choices: string;          // "A|B|C|D"
        correctAnswer: string;    // "A","B","C" ou "D"
        explanationChoices: string[];
    };
}

export default function Remediation() {
    const location = useLocation();
    const data = location.state as { id: string, modules: RemediationModule[] } | undefined;

    const [revealed, setRevealed] = useState<boolean[]>(
        Array(data?.modules?.length || 0).fill(false)
    );
    const [selected, setSelected] = useState<string[]>(
        Array(data?.modules?.length || 0).fill("")
    );

    // Si on n'a pas les modules, on remonte (ou affiche un message)
    if (!data?.modules) {
        return <Navigate to="/" replace />;
    }

    const { modules } = data;

    const onClick = (i: number, choice: string) => {
        if (revealed[i]) return;
        setRevealed(r => { const a = [...r]; a[i] = true; return a; });
        setSelected(s => { const a = [...s]; a[i] = choice; return a; });
    };

    return (
        <div className="evaluation-module">
            <Navbar />
            <h1>Remediation</h1>
            <div className="remediation-modules">
                {modules.map((mod, i) => {
                    const choices = mod.question.choices.split("|").map(c => c.trim());
                    return (
                        <div className="remediation-card" key={i}>
                            <h2 className="remediation-label">{mod.label}</h2>
                            <p className="remediation-lesson">{mod.lesson}</p>
                            <h3>{mod.question.prompt}</h3>
                            {/* Add choices explanation */}
                            { mod.question.explanationChoices.map((c) => <p>{c}</p>) }
                            <div className="remediation-choices">
                                {choices.map((c, idx) => {
                                    let cls = "choice-button";
                                    if (revealed[i]) {
                                        if (c === selected[i]) {
                                            cls += c === mod.question.correctAnswer ? " correct" : " wrong";
                                        } else if (selected[i] !== mod.question.correctAnswer
                                            && c === mod.question.correctAnswer) {
                                            cls += " correct";
                                        }
                                    }
                                    return (
                                        <button
                                            key={idx}
                                            className={cls}
                                            onClick={() => onClick(i, c)}
                                        >
                                            {c}
                                        </button>
                                    );
                                })}
                            </div>
                        </div>
                    );
                })}
            </div>
            <button className="submit-button" onClick={() => window.location.href = `/home/modules/${data.id}`}>Back to course</button>
        </div>
    );
}
