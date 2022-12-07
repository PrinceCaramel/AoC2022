using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day7 : IDay
    {
        #region Fields

        List<string> mInstructions = new List<string>();
        ElfFolder mRoot;

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            List<ElfFolder> lAllFolders = this.GetAllFolders(this.mRoot);
            int lResult = lAllFolders.Where(pFolder => pFolder.Size <= 100000).Select(pFolder => pFolder.Size).Sum();
            return lResult.ToString();
        }

        public string GetSecondPuzzle()
        {
            List<ElfFolder> lAllFolders = this.GetAllFolders(this.mRoot);
            int lUnusedSpace = 70000000 - this.mRoot.Size;
            return lAllFolders.Where(pFolder => lUnusedSpace + pFolder.Size >= 30000000).Select(pFolder => pFolder.Size).Min().ToString();
        }

        private List<ElfFolder> GetAllFolders(ElfFolder pFolder)
        {
            List<ElfFolder> lResult = new List<ElfFolder>();
            lResult.Add(pFolder);
            foreach (ElfFolder lChildFolder in pFolder.Children.OfType<ElfFolder>()) 
            {
                lResult.AddRange(this.GetAllFolders(lChildFolder));
            }
            return lResult;
        }

        public void ComputesData()
        {
            this.mInstructions = Utils.GetInputData(this).ToList();
            this.mRoot = new ElfFolder("/");
            ElfFolder lCurrentFolder = this.mRoot;
            foreach (string lInstruction in this.mInstructions) 
            {
                if (this.IsFile(lInstruction))
                {
                    string[] lSplit = lInstruction.Split(' ');
                    lCurrentFolder.AddChild(new ElfFile(lSplit[1], int.Parse(lSplit[0])));
                }
                else if (this.IsFolder(lInstruction))
                {
                    string[] lSplit = lInstruction.Split(' ');
                    lCurrentFolder.AddChild(new ElfFolder(lSplit[1]));
                }
                else if (this.IsCD(lInstruction))
                {
                    string lCD = lInstruction.Remove(0, 5);
                    if (lCD.Equals(".."))
                    {
                        lCurrentFolder = lCurrentFolder.Parent as ElfFolder;
                    }
                    else if (lCD.Equals("/"))
                    {
                        lCurrentFolder = this.mRoot;
                    }
                    else
                    {
                        ElfFolder lNewFolder = lCurrentFolder.GetFolder(lCD);
                        if (lNewFolder != null)
                        {
                            lCurrentFolder = lNewFolder;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
            }
        }

        private bool IsCD(string pInstruction) 
        {
            return pInstruction.StartsWith("$ cd");
        }

        private bool IsFolder(string pInstruction)
        {
            return pInstruction.StartsWith("dir");
        }

        public bool IsFile(string pInstruction)
        {
            return char.IsDigit(pInstruction.First());
        }

        #endregion Methods
    }

    public interface IFileSystemElement
    {
        int Size { get; }
    }

    public class ElfFolder : ATreeElement, IFileSystemElement
    {
        public ElfFolder(string pName)
        {
            this.Id = pName;
        }

        public int Size => this.Children.Select(pChild => (pChild as IFileSystemElement).Size).Sum();

        public ElfFolder GetFolder(string pId)
        {
            return this.Children.OfType<ElfFolder>().FirstOrDefault(pFolder => pFolder.Id.Equals(pId));
        }
    }

    public class ElfFile : ATreeElement, IFileSystemElement
    {
        public int Size { get; private set; }
        public ElfFile(string pFileName, int pSize)
        {
            this.Id = pFileName;
            this.Size = pSize;
        }

        public override void AddChild(ATreeElement pChild)
        {
            //Do nothing.
        }
    }
}
