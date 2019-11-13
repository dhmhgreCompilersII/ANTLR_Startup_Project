﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANTLR_Startup_Project {
    class ASTPrinter :ASTBaseVisitor<int> {
        private static int m_clusterSerial=0;
        private StreamWriter m_ostream;
        private string m_dotName;

        public ASTPrinter(string dotFileName) {
            m_ostream = new StreamWriter(dotFileName);
            m_dotName = dotFileName;
        }

        public override int VisitCompileUnit(CASTCompileUnit node) {

            m_ostream.WriteLine("digraph {");

            if (node.MChildren[node.GetContextIndex(contextType.CT_COMPILEUNIT_EXPRESSIONS)].Count != 0) {
                m_ostream.WriteLine("\tsubgraph cluster" + m_clusterSerial++ + "{");
                m_ostream.WriteLine("\t\tnode [style=filled,color=white];");
                m_ostream.WriteLine("\t\tstyle=filled;");
                m_ostream.WriteLine("\t\tcolor=lightgrey;");
                m_ostream.Write("\t\t");
                for (int i = 0; i < node.MChildren[0].Count; i++) {
                    m_ostream.Write(node.MChildren[0][i].MNodeName+";");
                    
                }
                m_ostream.WriteLine("\n\t\tlabel="+contextType.CT_COMPILEUNIT_EXPRESSIONS+";");
                m_ostream.WriteLine("\t}");
            }

            base.VisitCompileUnit(node);

            m_ostream.WriteLine("}");
            m_ostream.Close();

            // Prepare the process to run
            ProcessStartInfo start = new ProcessStartInfo();
            // Enter in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = "-Tgif " + m_dotName +" -o" +m_dotName +".gif";
            // Enter the executable to run, including the complete path
            start.FileName = "dot";
            // Do you want to show a console window?
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;


            // Run the external process & wait for it to finish
            using (Process proc = Process.Start(start)) {
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }

            return 0;
        }

        public override int VisitIDENTIFIER(CASTIDENTIFIER node) {
            return base.VisitIDENTIFIER(node);
        }

        public override int VisitNUMBER(CASTNUMBER node) {
            return base.VisitNUMBER(node);
        }

        public override int VisitAddition(CASTAddition node) {

            if (node.MChildren[node.GetContextIndex(contextType.CT_ADDITION_LEFT)].Count != 0) {
                m_ostream.WriteLine("\tsubgraph cluster" + m_clusterSerial++ + "{");
                m_ostream.WriteLine("\t\tnode [style=filled,color=white];");
                m_ostream.WriteLine("\t\tstyle=filled;");
                m_ostream.WriteLine("\t\tcolor=lightgrey;");
                m_ostream.Write("\t\t");
                for (int i = 0; i < node.MChildren[0].Count; i++) {
                    m_ostream.Write(node.MChildren[0][i].MNodeName + ";");
                }
                m_ostream.WriteLine("\n\t\tlabel=" + contextType.CT_ADDITION_LEFT + ";");
                m_ostream.WriteLine("\t}");
            }

            if (node.MChildren[node.GetContextIndex(contextType.CT_ADDITION_RIGHT)].Count != 0) {
                m_ostream.WriteLine("\tsubgraph cluster" + m_clusterSerial++ + "{");
                m_ostream.WriteLine("\t\tnode [style=filled,color=white];");
                m_ostream.WriteLine("\t\tstyle=filled;");
                m_ostream.WriteLine("\t\tcolor=lightgrey;");
                m_ostream.Write("\t\t");
                for (int i = 0; i < node.MChildren[0].Count; i++) {
                    m_ostream.Write(node.MChildren[0][i].MNodeName + ";");
                }
                m_ostream.WriteLine("\n\t\tlabel=" + contextType.CT_ADDITION_RIGHT + ";");
                m_ostream.WriteLine("\t}");
            }

            base.VisitAddition(node);

            m_ostream.WriteLine("{0}->{1}",node.MParent.MNodeName,node.MNodeName);

            return 0;
        }

        public override int VisitSubtraction(CASTSubtraction node) {
            return base.VisitSubtraction(node);
        }

        public override int VisitMultiplication(CASTMultiplication node) {
            return base.VisitMultiplication(node);
        }

        public override int VisitDivision(CASTDivision node) {
            return base.VisitDivision(node);
        }

        public override int VisitAssignment(CASTAssignment node) {
            return base.VisitAssignment(node);
        }
    }
}
