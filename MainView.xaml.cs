using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;


namespace BED_Calc
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        // specify properties, including one for the open script context
        private int numberOfFractions;
        private string organName;
        private float alphaBetaRatio;
        public float bed;
        public float eqd2;
        private PlanSetup planSetup;

        private ScriptContext context;

        public MainView(ScriptContext context)
        {
            InitializeComponent();
            this.context = context; //Copy currently loaded patient data content
            planSetup = context.PlanSetup;
            numberOfFractions= (int)planSetup.NumberOfFractions; //extract number of fractions from open plancontext

        } 
            public MainView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            organName=OrganNameTB.Text;
            float.TryParse(organName, out alphaBetaRatio);
            alphaBetaRatio = float.Parse(AlphaBetaRatioTB.Text);

            StructureSet structureSet = planSetup.StructureSet;
            if (structureSet == null)
                throw new ApplicationException("The selected plan does not reference a StructureSet.");

            //find structure if it exists, if it does, set target equal to that structure. throw exce
            Structure target = null;

            bool StructureEmpty = false;
            foreach (var structure in structureSet.Structures)
                {   
                    if (structure.Id == organName)
                    {
                        if (structure.IsEmpty)
                        {
                            
                            MessageBox.Show(String.Format("Structure {0} is empty, try another structure", organName));
                            target = null;
                            break;
                        }
                        else
                        {
                            target = structure;
                            break;
                        }

                    }
                }
            
            if (target == null && !StructureEmpty)
                MessageBox.Show(String.Format("I can't find a structure called {0}", organName));
            
            else if (DoseTypeTB.Text.Equals("Mean")) //if target is not null and structure is not empty then we can retriev our dose, we split into cases 
            {
                DVHData dvhData = planSetup.GetDVHCumulativeData(target,
                                        DoseValuePresentation.Absolute,
                                        VolumePresentation.AbsoluteCm3, 0.1);
                float dose = (float)dvhData.MeanDose.Dose;//compute mean dose            
                float dosePerFraction = dose / numberOfFractions;
                bed = dose * (1.0f + dosePerFraction / alphaBetaRatio);
                eqd2 = dose * ((dosePerFraction + alphaBetaRatio) / (2.0f + alphaBetaRatio));

                BEDTB.Text = bed.ToString();
                EQD2TB.Text = eqd2.ToString();

                /*this is for debugging only
                MessageBox.Show(String.Format("Organ={0}, MeanDose={1}," +
                    "Dose Per Fraction={2}, Number of Fractions= {3}", organName, dose,dosePerFraction, numberOfFractions));*/
            }

            else if (DoseTypeTB.Text.Equals("Max")) //if target is not null and structure is not empty then we can retriev our dose, we split into cases 
            {
                DVHData dvhData = planSetup.GetDVHCumulativeData(target,
                                        DoseValuePresentation.Absolute,
                                        VolumePresentation.AbsoluteCm3, 0.1);
                float dose = (float)dvhData.MaxDose.Dose;//compute mean dose            
                float dosePerFraction = dose / numberOfFractions;
                bed = dose * (1.0f + dosePerFraction / alphaBetaRatio);
                eqd2 = dose * ((dosePerFraction + alphaBetaRatio) / (2.0f + alphaBetaRatio));

                BEDTB.Text = bed.ToString();
                EQD2TB.Text = eqd2.ToString();

                /*this is for debugging only
                MessageBox.Show(String.Format("Organ={0}, MeanDose={1}," +
                    "Dose Per Fraction={2}, Number of Fractions= {3}", organName, dose,dosePerFraction, numberOfFractions));*/
            }
            else if (DoseTypeTB.Text.Equals("Min")) //if target is not null and structure is not empty then we can retriev our dose, we split into cases 
            {
                DVHData dvhData = planSetup.GetDVHCumulativeData(target,
                                        DoseValuePresentation.Absolute,
                                        VolumePresentation.AbsoluteCm3, 0.1);
                float dose = (float)dvhData.MinDose.Dose;//compute mean dose            
                float dosePerFraction = dose / numberOfFractions;
                bed = dose * (1.0f + dosePerFraction / alphaBetaRatio);
                eqd2 = dose * ((dosePerFraction + alphaBetaRatio) / (2.0f + alphaBetaRatio));

                BEDTB.Text = bed.ToString();
                EQD2TB.Text = eqd2.ToString();

                /*this is for debugging only
                MessageBox.Show(String.Format("Organ={0}, MeanDose={1}," +
                    "Dose Per Fraction={2}, Number of Fractions= {3}", organName, dose,dosePerFraction, numberOfFractions));*/
            }
            else if (DoseTypeTB.Text.EndsWith("cc"))
            {
                MessageBox.Show("Still working on retriving cc doses");
            }
            else if (DoseTypeTB.Text.EndsWith("%"))
            {
                MessageBox.Show("Still working on retriving % doses");
            }
            else
            {
                MessageBox.Show("Invalid Dose type, please enter either enter either \"Mean\", \"Max\", \"Min\", \"DNcc\" or \"DN%\" into the Input Dose box.");
            }


        }


        /*private void OrganNameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            OrganName=OrganNameTB.Text;
            MessageBox.Show(String.Format("Hello, the organ name is {0}",OrganName));
        }*/
    }
}
