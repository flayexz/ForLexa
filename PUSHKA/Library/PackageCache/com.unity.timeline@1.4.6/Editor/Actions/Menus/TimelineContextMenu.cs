1 - (alo * bhi); err3 = err2 - (ahi * blo); ti0 = (alo * blo) - err3;
                    tj1 = (double)(cdx * adytail); c = (double)(splitter * cdx); abig = (double)(c - cdx); ahi = c - abig; alo = cdx - ahi; c = (double)(splitter * adytail); abig = (double)(c - adytail); bhi = c - abig; blo = adytail - bhi; err1 = tj1 - (ahi * bhi); err2 = err1 - (alo * bhi); err3 = err2 - (ahi * blo); tj0 = (alo * blo) - err3;
                    _i = (double)(ti0 + tj0); bvirt = (double)(_i - ti0); avirt = _i - bvirt; bround = tj0 - bvirt; around = ti0 - avirt; u[0] = around + bround; _j = (double)(ti1 + _i); bvirt = (double)(_j - ti1); avirt = _j - bvirt; bround = _i - bvirt; around = ti1 - avirt; _0 = around + bround; _i = (double)(_0 + tj1); bvirt = (double)(_i - _0); avirt = _i - bvirt; bround = tj1 - bvirt; around = _0 - avirt; u[1] = around + bround; u3 = (double)(_j + _i); bvirt = (double)(u3 - _j); avirt = u3 - bvirt; bround = _i - bvirt; around = _j - avirt; u[2] = around + bround;
                    u[3] = u3;
                    negate = -cdy;
                    ti1 = (double)(adxtail * negate); c = (double)(splitter * adxtail); abig = (double)(c - adxtail); ahi = c - abig; alo = adxtail - ahi; c = (double)(splitter * negate); abig = (double)(c - negate); bhi = c - abig; blo = negate - bhi; err1 = ti1 - (ahi * bhi); err2 = err1 - (alo * bhi); err3 = err2 - (ahi * blo); ti0 = (alo * blo) - err3;
                    negate = -cdytail;
                    tj1 = (double)(adx * negate); c = (double)(splitter * adx); abig = (double)(c - adx); ahi = c - abig; alo = adx - ahi; c = (double)(splitter * negate); abig = (double)(c - negate); bhi = c - abig; blo = negate - bhi; err1 = tj1 - (ahi * bhi); err2 = err1 - (alo * bhi); err3 = err2 - (ahi * blo); tj0 = (alo * blo) - err3;
                    _i = (double)(ti0 + tj0); bvirt = (double)(_i - ti0); avirt = _i - bvirt; bround = tj0 - bvirt; around = ti0 - avirt; v[0] = around + bround; _j = (double)(ti1 + _i); bvirt = (double)(_j - ti1); avirt = _j - bvirt; bround = _i - bvirt; around = ti1 - avirt; _0 = around + bround; _i = (double)(_0 + tj1); bvirt = (double)(_i - _0); avirt = _i - bvirt; bround = tj1 - bvirt; around = _0 - avirt; v[1] = around + bround; v3 = (double)(_j + _i); bvirt = (double)(v3 - _j); avirt = v3 - bvirt; bround = _i - bvirt; around = _j - avirt; v[2] = around + bround;
                    v[3] = v3;
                    catlen = FastExpansionSumZeroElim(4, u, 4, v, cat);

                    ti1 = (double)(cdxtail * adytail); c = (double)(splitter * cdxtail); abig = (double)(c - cdxtail); ahi = c - abig; alo = cdxtail - ahi; c = (double)(splitter * adytail); abig = (double)(c - adytail); bhi = c - abig; blo = adytail - bhi; err1 = ti1 - (ahi * bhi); err2 = err1 - (alo * bhi); err3 = err2 - (ahi * blo); ti0 = (alo * blo) - err3;
                    tj1 = (double)(adxtail * cdytail); c = (double)(splitter * adxtail); abig = (double)(c - adxtail); ahi = c - abig; alo = adxtail - ahi; c = (double)(splitter * cdytail); abig = (double)(c - cdytail); bhi = c - abig; blo = cdytail - bhi; err1 = tj1 - (ahi * bhi); err2 = err1 - (alo * bhi); err3 = err2 - (ahi * blo); tj0 = (alo * blo) - err3;
                    _i = (double)(ti0 - tj0); bvirt = (double)(ti0 - _i); avirt = _i + bvirt; bround = bvirt - tj0; around = ti0 - avirt; catt[0] = around + bround; _j = (double)(ti1 + _i); bvirt = (double)(_j - ti1); avirt = _j - bvirt; bround = _i - bvirt; around = ti1 - avirt; _0 = around + bround; _i = (double)(_0 - tj1); bvirt = (double)(_0 - _i); avirt = _i + bvirt; bround = bvirt - tj1; around = _0 - avirt; catt[1] = around + bround; catt3 = (double)(_j + _i); bvirt = (double)(catt3 - _j); avirt = catt3 - bvirt; bround = _i - bvirt; around = _j - avirt; catt[2] = around + bround;
                    catt[3] = catt3;
                    cattlen = 4;
                }
                else
                {
                    cat[0] = 0.0;
                    catlen = 1;
                    catt[0] = 0.0;
                    cattlen = 1;
                }

                if (bdxtail != 0.0)
                {
                    temp16alen = ScaleExpansionZeroElim(bxtcalen, bxtca, bdxtail, temp16a);
                    bxtcatlen = ScaleExpansionZeroElim(catlen, cat, bdxtail, bxtcat);
                    temp32alen = ScaleExpansionZeroElim(bxtcatlen, bxtcat, 2.0 * bdx, temp32a);
                    temp48len = FastExpansionSumZeroElim(temp16alen, temp16a, temp32alen, temp32a, temp48);
                    finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
                    finswap = finnow; finnow = finother; finother = finswap;
                    if (cdytail != 0.0)
                    {
                        temp8len = ScaleExpansionZeroElim(4, aa, bdxtail, temp8);
                        temp16alen = ScaleExpansionZeroElim(temp8len, temp8, cdytail, temp16a);
                        finlength = FastExpansionSumZeroElim(finlength, finnow, temp16alen, temp16a, finother);
                        finswap = finnow; finnow = finother; finother = finswap;
                    }
                    if (adytail != 0.0)
                    {
                        temp8len = ScaleExpansionZeroElim(4, cc, -bdxtail, temp8);
                        temp16alen = ScaleExpansionZeroElim(temp8len, temp8, adytail, temp16a);
                        finlength = FastExpansionSumZeroElim(finlength, finnow, temp16alen, temp16a, finother);
                        finswap = finnow; finnow = finother; finother = finswap;
                    }

                    temp32alen = ScaleExpansionZeroElim(bxtcatlen, bxtcat, bdxtail, temp32a);
                    bxtcattlen = ScaleExpansionZeroElim(cattlen, catt, bdxtail, bxtcatt);
                    temp16alen = ScaleExpansionZeroElim(bxtcattlen, bxtcatt, 2.0 * bdx, temp16a);
                    temp16blen = ScaleExpansionZeroElim(bxtcattlen, bxtcatt, bdxtail, temp16b);
                    temp32blen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32b);
                    temp64len = FastExpansionSumZeroElim(temp32alen, temp32a, temp32blen, temp32b, temp64);
                    finlength = FastExpansionSumZeroElim(finlength, finnow, temp64len, temp64, finother);
                    finswap = finnow; finnow = finother; finother = finswap;
                }
                if (bdytail != 0.0)
                {
                    temp16alen = ScaleExpansionZeroElim(bytcalen, bytca, bdytail, temp16a);
                    bytcatlen = ScaleExpansionZeroElim(catlen, cat, bdytail, bytcat);
                    temp32alen = ScaleExpansionZeroElim(bytcatlen, bytcat, 2.0 * bdy, temp32a);
                    temp48len = FastExpansionSumZeroElim(temp16alen, temp16a, temp32alen, temp32a, temp48);
                    finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
                    finswap = finnow; finnow = finother; finother = finswap;

                    temp32alen = ScaleExpansionZeroElim(bytcatlen, bytcat, bdytail, temp32a);
                    bytcattlen = ScaleExpansionZeroElim(cattlen, catt, bdytail, bytcatt);
                    temp16alen = ScaleExpansionZeroElim(bytcattlen, bytcatt, 2.0 * bdy, temp16a);
                    temp16blen = ScaleExpansionZeroElim(bytcattlen, bytcatt, bdytail, temp16b);
                    temp32blen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32b);
                    temp64len = FastExpansionSumZeroElim(temp32alen, temp32a, temp32blen, temp32b, temp64);
                    finlength = FastExpansionSumZeroElim(finlength, finnow, temp64len, temp64, finother);
                    finswap = finnow; finnow = finother; finother = finswap;
                }
            }
            if ((cdxtail != 0.0) || (cdytail != 0.0))
            {
                if ((adxtail != 0.0) || (adytail != 0.0)
                    || (bdxtail != 0.0) || (bdytail != 0.0))
                {
                    ti1 = (double)(adxtail * bdy); c = (double)(splitter * adxtail); abig = (double)(c - adxtail); ahi = c - abig; alo = adxtail - ahi; c = (double)(splitter * bdy); abig = (double)(c - bdy); bhi = c - abig; blo = bdy - bhi; err1 = ti1 - (ahi * bhi); err2 = err1 - (alo * bhi); err3 = err2 - (ahi * blo); ti0 = (alo * blo) - err3;
                    tj1 = (double)(adx * bdytail); c = (double)(splitter * adx); abig = (double)(c - adx); ahi = c - abig; alo = adx - ahi; c = (double)(splitter * bdytail); abig = (double)(c - bdytail); bhi = c - abig; blo = bdytail - bhi; err1 = tj1 - (ahi * bhi); err2 = err1 - (alo * bhi); err3 = err2 - (ahi * blo); tj0 = (alo * blo) - err3;
                    _i = (double)(ti0 + tj0); bvirt = (double)(_i - ti0); avirt = _i - bvirt; bround = tj0 - bvirt; around = ti0 - avirt; u[0] = around + bround; _j = (double)(ti1 + _i); bvirt = (double)(_j - ti1); avirt = _j - bvirt; bround = _i - bvirt; around = ti1 - avirt; _0 = around + bround; _i = (double)(_0 + tj1); bvirt = (double)(_i - _0); avirt = _i - bvirt; bround = tj1 - bvirt; around = _0 - avirt; u[1] = around + bround; u3 = (double)(_j + _i); bvirt = (double)(u3 - _j); avirt = u3 - bvirt; bround = _i - bvirt; around = _j - avirt; u[2] = around + bround;
                    u[3] = u3;
                    negate = -ady;
                    ti1 = (double)(bdxtail * negate); c = (double)(splitter * bdxtail); abig = (double)(c - bdxtail); ahi = c - abig; alo = bdxtail - ahi; c = (double)(splitter * negate); abig = (double)(c - negate); bhi = c - abig; blo = negate - bhi; err1 = ti1 - (ahi * bhi); err2 = err1 - (alo * bhi); err3 = err2 - (ahi * blo); ti0 = (alo * blo) - err3;
                    negate = -adytail;
                    tj1 = (double)(bdx * negate); c = (double)(splitter * bdx); abig = (double)(c - bdx); ahi = c - abig; alo = bdx - ahi; c = (double)(splitter * negate); abig = (double)(c - negate); bhi = c - abig; blo = negate - bhi; err1 = tj1 - (ahi * bhi); err2 = err1 - (alo * bhi); err3 = err2 - (ahi * blo); tj0 = (alo * blo) - err3;
                    _i = (double)(ti0 + tj0); bvirt = (double)(_i - ti0); avirt = _i - bvirt; bround = tj0 - bvirt; around = ti0 - avirt; v[0] = around + bround; _j = (double)(ti1 + _i); bvirt = (double)(_j - ti1); avirt = _j - bvirt; bround = _i - bvirt; around = ti1 - avirt; _0 = around + bround; _i = (double)(_0 + tj1); bvirt = (double)(_i - _0); avirt = _i - bvirt; bround = tj1 - bvirt; around = _0 - avirt; v[1] = around + bround; v3 = (double)(_j + _i); bvirt = (double)(v3 - _j); avirt = v3 - bvirt; bround = _i - bvirt; around = _j - avirt; v[2] = around + bround;
                    v[3] = v3;
                    abtlen = FastExpansionSumZeroElim(4, u, 4, v, abt);

                    ti1 = (double)(adxtail * bdytail); c = (double)(splitter * adxtail); abig = (double)(c - adxtail); ahi = c - abig; alo = adxtail - ahi; c = (double)(splitter * bdytail); abig = (double)(c - bdytail); bhi = c - abig; blo = bdytail - bhi; err1 = ti1 - (ahi * bhi); err2 = err1 - (alo * bhi); err3 = err2 - (ahi * blo); ti0 = (alo * blo) - err3;
                    tj1 = (double)(bdxtail * adytail); c = (double)(splitter * bdxtail); abig = (double)(c - bdxtail); ahi = c - abig; alo = bdxtail - ahi; c = (double)(splitter * adytail); abig = (double)(c - adytail); bhi = c - abig; blo = adytail - bhi; err1 = tj1 - (ahi * bhi); err2 = err1 - (alo * bhi); err3 = err2 - (ahi * blo); tj0 = (alo * blo) - err3;
                    _i = (double)(ti0 - tj0); bvirt = (double)(ti0 - _i); avirt = _i + bvirt; bround = bvirt - tj0; around = ti0 - avirt; abtt[0] = around + bround; _j = (double)(ti1 + _i); bvirt = (double)(_j - ti1); avirt = _j - bvirt; bround = _i - bvirt; around = ti1 - avirt; _0 = around + bround; _i = (double)(_0 - tj1); bvirt = (double)(_0 - _i); avirt = _i + bvirt; bround = bvirt - tj1; around = _0 - avirt; abtt[1] = around + bround; abtt3 = (double)(_j + _i); bvirt = (double)(abtt3 - _j); avirt = abtt3 - bvirt; bround = _i - bvirt; around = _j - avirt; abtt[2] = around + bround;
                    abtt[3] = abtt3;
                    abttlen = 4;
                }
                else
                {
                    abt[0] = 0.0;
                    abtlen = 1;
                    abtt[0] = 0.0;
                    abttlen = 1;
                }

                if (cdxtail != 0.0)
                {
                    temp16alen = ScaleExpansionZeroElim(cxtablen, cxtab, cdxtail, temp16a);
                    cxtabtlen = ScaleExpansionZeroElim(abtlen, abt, cdxtail, cxtabt);
                    temp32alen = ScaleExpansionZeroElim(cxtabtlen, cxtabt, 2.0 * cdx, temp32a);
                    temp48len = FastExpansionSumZeroElim(temp16alen, temp16a, temp32alen, temp32a, temp48);
                    finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
                    finswap = finnow; finnow = finother; finother = finswap;
                    if (adytail != 0.0)
                    {
                        temp8len = ScaleExpansionZeroElim(4, bb, cdxtail, temp8);
                        temp16alen = ScaleExpansionZeroElim(temp8len, temp8, adytail, temp16a);
                        finlength = FastExpansionSumZeroElim(finlength, finnow, temp16alen, temp16a, finother);
                        finswap = finnow; finnow = finother; finother = finswap;
                    }
                    if (bdytail != 0.0)
                    {
                        temp8len = ScaleExpansionZeroElim(4, aa, -cdxtail, temp8);
                        temp16alen = ScaleExpansionZeroElim(temp8len, temp8, bdytail, temp16a);
                        finlength = FastExpansionSumZeroElim(finlength, finnow, temp16alen, temp16a, finother);
                        finswap = finnow; finnow = finother; finother = finswap;
                    }

                    temp32alen = ScaleExpansionZeroElim(cxtabtlen, cxtabt, cdxtail, temp32a);
                    cxtabttlen = ScaleExpansionZeroElim(abttlen, abtt, cdxtail, cxtabtt);
                    temp16alen = ScaleExpansionZeroElim(cxtabttlen, cxtabtt, 2.0 * cdx, temp16a);
                    temp16blen = ScaleExpansionZeroElim(cxtabttlen, cxtabtt, cdxtail, temp16b);
                    temp32blen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32b);
                    temp64len = FastExpansionSumZeroElim(temp32alen, temp32a, temp32blen, temp32b, temp64);
                    finlength = FastExpansionSumZeroElim(finlength, finnow, temp64len, temp64, finother);
                    finswap = finnow; finnow = finother; finother = finswap;
                }
                if (cdytail != 0.0)
                {
                    temp16alen = ScaleExpansionZeroElim(cytablen, cytab, cdytail, temp16a);
                    cytabtlen = ScaleExpansionZeroElim(abtlen, abt, cdytail, cytabt);
                    temp32alen = ScaleExpansionZeroElim(cytabtlen, cytabt, 2.0 * cdy, temp32a);
                    temp48len = FastExpansionSumZeroElim(temp16alen, temp16a, temp32alen, temp32a, temp48);
                    finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
                    finswap = finnow; finnow = finother; finother = finswap;


                    temp32alen = ScaleExpansionZeroElim(cytabtlen, cytabt, cdytail, temp32a);
                    cytabttlen = ScaleExpansionZeroElim(abttlen, abtt, cdytail, cytabtt);
                    temp16alen = ScaleExpansionZeroElim(cytabttlen, cytabtt, 2.0 * cdy, temp16a);
                    temp16blen = ScaleExpansionZeroElim(cytabttlen, cytabtt, cdytail, temp16b);
                    temp32blen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32b);
                    temp64len = FastExpansionSumZeroElim(temp32alen, temp32a, temp32blen, temp32b, temp64);
                    finlength = FastExpansionSumZeroElim(finlength, finnow, temp64len, temp64, finother);
                    finswap = finnow; finnow = finother; finother = finswap;
                }
            }

            return finnow[finlength - 1];
        }

        #region Workspace

        // InCircleAdapt workspace:
        double[] fin1, fin2, abdet;

        double[] axbc, axxbc, aybc, ayybc, adet;
        double[] bxca, bxxca, byca, byyca, bdet;
        double[] cxab, cxxab, cyab, cyyab, cdet;

        double[] temp8, temp16a, temp16b, temp16c;
        double[] temp32a, temp32b, temp48, temp64;

        private void AllocateWorkspace()
        {
            fin1 = new double[1152];
            fin2 = new double[1152];
            abdet = new double[64];

            axbc = new double[8];
            axxbc = new double[16];
            aybc = new double[8];
            ayybc = new double[16];
            adet = new double[32];

            bxca = new double[8];
            bxxca = new double[16];
            byca = new double[8];
            byyca = new double[16];
            bdet = new double[32];

            cxab = new double[8];
            cxxab = new double[16];
            cyab = new double[8];
            cyyab = new double[16];
            cdet = new double[32];

            temp8 = new double[8];
            temp16a = new double[16];
            temp16b = new double[16];
            temp16c = new double[16];

            temp32a = new double[32];
            temp32b = new double[32];
            temp48 = new double[48];
            temp64 = new double[64];
        }

        private void ClearWorkspace()
        {
        }

        #endregion

        #endregion
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           